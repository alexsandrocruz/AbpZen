import { google } from 'googleapis';
import { storage } from './storage';

let connectionSettings: any;
let tokenFetchedAt: number = 0;

async function getAccessToken(forceRefresh = false) {
  const now = Date.now();
  const expiresAt = connectionSettings?.settings?.expires_at 
    ? new Date(connectionSettings.settings.expires_at).getTime() 
    : 0;
  
  const tokenValid = connectionSettings 
    && expiresAt > now + 60000 
    && (now - tokenFetchedAt) < 3600000;
  
  if (tokenValid && !forceRefresh) {
    return connectionSettings.settings.access_token || connectionSettings.settings?.oauth?.credentials?.access_token;
  }
  
  const hostname = process.env.REPLIT_CONNECTORS_HOSTNAME;
  const xReplitToken = process.env.REPL_IDENTITY 
    ? 'repl ' + process.env.REPL_IDENTITY 
    : process.env.WEB_REPL_RENEWAL 
    ? 'depl ' + process.env.WEB_REPL_RENEWAL 
    : null;

  if (!xReplitToken) {
    throw new Error('X_REPLIT_TOKEN not found for repl/depl');
  }

  connectionSettings = await fetch(
    'https://' + hostname + '/api/v2/connection?include_secrets=true&connector_names=google-calendar',
    {
      headers: {
        'Accept': 'application/json',
        'X_REPLIT_TOKEN': xReplitToken
      }
    }
  ).then(res => res.json()).then(data => data.items?.[0]);
  
  tokenFetchedAt = Date.now();

  const accessToken = connectionSettings?.settings?.access_token || connectionSettings?.settings?.oauth?.credentials?.access_token;

  if (!connectionSettings || !accessToken) {
    throw new Error('Google Calendar not connected');
  }
  return accessToken;
}

async function withTokenRefresh<T>(operation: () => Promise<T>): Promise<T> {
  try {
    return await operation();
  } catch (error: any) {
    if (error?.code === 401 || error?.message?.includes('invalid_grant') || error?.message?.includes('Token has been expired')) {
      connectionSettings = null;
      await getAccessToken(true);
      return await operation();
    }
    throw error;
  }
}

export async function getGoogleCalendarClient() {
  const accessToken = await getAccessToken();

  const oauth2Client = new google.auth.OAuth2();
  oauth2Client.setCredentials({
    access_token: accessToken
  });

  return google.calendar({ version: 'v3', auth: oauth2Client });
}

// Per-workspace Google Calendar client
export async function getWorkspaceGoogleCalendarClient(workspaceId: string) {
  const credentials = await storage.getWorkspaceGoogleCalendarCredentials(workspaceId);
  
  if (!credentials) {
    throw new Error('Google Calendar não conectado para este workspace');
  }
  
  const clientId = process.env.GOOGLE_CLIENT_ID;
  const clientSecret = process.env.GOOGLE_CLIENT_SECRET;
  
  if (!clientId || !clientSecret) {
    throw new Error('Credenciais do Google OAuth não configuradas');
  }
  
  const oauth2Client = new google.auth.OAuth2(clientId, clientSecret);
  
  oauth2Client.setCredentials({
    access_token: credentials.accessToken,
    refresh_token: credentials.refreshToken,
  });
  
  // Refresh token if needed
  if (credentials.expiresAt && new Date(credentials.expiresAt) < new Date()) {
    try {
      const { credentials: newTokens } = await oauth2Client.refreshAccessToken();
      await storage.saveWorkspaceGoogleCalendarCredentials({
        workspaceId,
        accessToken: newTokens.access_token!,
        refreshToken: newTokens.refresh_token || credentials.refreshToken,
        expiresAt: newTokens.expiry_date ? new Date(newTokens.expiry_date) : null,
        scopes: credentials.scopes,
        googleEmail: credentials.googleEmail,
      });
      oauth2Client.setCredentials(newTokens);
    } catch (refreshError) {
      await storage.deleteWorkspaceGoogleCalendarCredentials(workspaceId);
      throw new Error('Sessão expirada. Reconecte sua conta do Google.');
    }
  }
  
  return google.calendar({ version: 'v3', auth: oauth2Client });
}

export async function getFreeBusySlots(timeMin: Date, timeMax: Date): Promise<Array<{start: Date, end: Date}>> {
  try {
    return await withTokenRefresh(async () => {
      const calendar = await getGoogleCalendarClient();
      const response = await calendar.freebusy.query({
        requestBody: {
          timeMin: timeMin.toISOString(),
          timeMax: timeMax.toISOString(),
          items: [{ id: 'primary' }],
        },
      });

      const busySlots = response.data.calendars?.primary?.busy || [];
      return busySlots.map(slot => ({
        start: new Date(slot.start!),
        end: new Date(slot.end!),
      }));
    });
  } catch (error) {
    console.error('Error fetching free/busy:', error);
    return [];
  }
}

// Per-workspace version
export async function getWorkspaceFreeBusySlots(workspaceId: string, timeMin: Date, timeMax: Date): Promise<Array<{start: Date, end: Date}>> {
  try {
    const calendar = await getWorkspaceGoogleCalendarClient(workspaceId);
    const response = await calendar.freebusy.query({
      requestBody: {
        timeMin: timeMin.toISOString(),
        timeMax: timeMax.toISOString(),
        items: [{ id: 'primary' }],
      },
    });

    const busySlots = response.data.calendars?.primary?.busy || [];
    return busySlots.map(slot => ({
      start: new Date(slot.start!),
      end: new Date(slot.end!),
    }));
  } catch (error) {
    console.error('Error fetching free/busy:', error);
    return [];
  }
}

export async function createCalendarEvent(event: {
  summary: string;
  description?: string;
  startTime: Date;
  endTime: Date;
  attendeeEmail?: string;
  createMeetLink?: boolean;
}): Promise<{ eventId: string; meetLink?: string } | null> {
  try {
    return await withTokenRefresh(async () => {
      const calendar = await getGoogleCalendarClient();
      
      const eventData: any = {
        summary: event.summary,
        description: event.description,
        start: {
          dateTime: event.startTime.toISOString(),
          timeZone: 'America/Sao_Paulo',
        },
        end: {
          dateTime: event.endTime.toISOString(),
          timeZone: 'America/Sao_Paulo',
        },
      };

      if (event.attendeeEmail) {
        eventData.attendees = [{ email: event.attendeeEmail }];
      }

      if (event.createMeetLink) {
        eventData.conferenceData = {
          createRequest: {
            requestId: `meet-${Date.now()}`,
            conferenceSolutionKey: { type: 'hangoutsMeet' },
          },
        };
      }

      const response = await calendar.events.insert({
        calendarId: 'primary',
        requestBody: eventData,
        conferenceDataVersion: event.createMeetLink ? 1 : 0,
        sendUpdates: 'all',
      });

      return {
        eventId: response.data.id!,
        meetLink: response.data.conferenceData?.entryPoints?.find(e => e.entryPointType === 'video')?.uri ?? undefined,
      };
    });
  } catch (error) {
    console.error('Error creating calendar event:', error);
    return null;
  }
}

// Per-workspace version
export async function createWorkspaceCalendarEvent(workspaceId: string, event: {
  summary: string;
  description?: string;
  startTime: Date;
  endTime: Date;
  attendeeEmail?: string;
  createMeetLink?: boolean;
}): Promise<{ eventId: string; meetLink?: string } | null> {
  try {
    const calendar = await getWorkspaceGoogleCalendarClient(workspaceId);
    
    const eventData: any = {
      summary: event.summary,
      description: event.description,
      start: {
        dateTime: event.startTime.toISOString(),
        timeZone: 'America/Sao_Paulo',
      },
      end: {
        dateTime: event.endTime.toISOString(),
        timeZone: 'America/Sao_Paulo',
      },
    };

    if (event.attendeeEmail) {
      eventData.attendees = [{ email: event.attendeeEmail }];
    }

    if (event.createMeetLink) {
      eventData.conferenceData = {
        createRequest: {
          requestId: `meet-${Date.now()}`,
          conferenceSolutionKey: { type: 'hangoutsMeet' },
        },
      };
    }

    const response = await calendar.events.insert({
      calendarId: 'primary',
      requestBody: eventData,
      conferenceDataVersion: event.createMeetLink ? 1 : 0,
      sendUpdates: 'all',
    });

    return {
      eventId: response.data.id!,
      meetLink: response.data.conferenceData?.entryPoints?.find(e => e.entryPointType === 'video')?.uri ?? undefined,
    };
  } catch (error) {
    console.error('Error creating calendar event:', error);
    return null;
  }
}

export async function deleteCalendarEvent(eventId: string): Promise<boolean> {
  try {
    return await withTokenRefresh(async () => {
      const calendar = await getGoogleCalendarClient();
      await calendar.events.delete({
        calendarId: 'primary',
        eventId,
        sendUpdates: 'all',
      });
      return true;
    });
  } catch (error) {
    console.error('Error deleting calendar event:', error);
    return false;
  }
}

// Per-workspace version
export async function deleteWorkspaceCalendarEvent(workspaceId: string, eventId: string): Promise<boolean> {
  try {
    const calendar = await getWorkspaceGoogleCalendarClient(workspaceId);
    await calendar.events.delete({
      calendarId: 'primary',
      eventId,
      sendUpdates: 'all',
    });
    return true;
  } catch (error) {
    console.error('Error deleting calendar event:', error);
    return false;
  }
}

export async function isCalendarConnected(): Promise<boolean> {
  try {
    await getGoogleCalendarClient();
    return true;
  } catch {
    return false;
  }
}

// Per-workspace version
export async function isWorkspaceCalendarConnected(workspaceId: string): Promise<boolean> {
  try {
    const credentials = await storage.getWorkspaceGoogleCalendarCredentials(workspaceId);
    return !!credentials && (!!credentials.refreshToken || (!!credentials.accessToken && (!credentials.expiresAt || new Date(credentials.expiresAt) > new Date())));
  } catch {
    return false;
  }
}

export async function getGoogleContactsClient() {
  const accessToken = await getAccessToken();

  const oauth2Client = new google.auth.OAuth2();
  oauth2Client.setCredentials({
    access_token: accessToken
  });

  return google.people({ version: 'v1', auth: oauth2Client });
}
