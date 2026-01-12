import { storage } from "./storage";
import { workflowEngine } from "./workflow-engine";

const CHECK_INTERVAL = 1000 * 60 * 60; // Run every hour

function getLocalDateString(date: Date): string {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

function getDaysDifference(now: Date, dueDate: Date): number {
  const nowStr = getLocalDateString(now);
  const dueStr = getLocalDateString(dueDate);
  
  const nowParts = nowStr.split('-').map(Number);
  const dueParts = dueStr.split('-').map(Number);
  
  const nowMidnight = new Date(nowParts[0], nowParts[1] - 1, nowParts[2], 0, 0, 0, 0);
  const dueMidnight = new Date(dueParts[0], dueParts[1] - 1, dueParts[2], 0, 0, 0, 0);
  
  const diffMs = dueMidnight.getTime() - nowMidnight.getTime();
  return Math.floor(diffMs / (1000 * 60 * 60 * 24));
}

async function checkTaskDeadlines() {
  try {
    const now = new Date();
    console.log('[TaskReminder] Checking task deadlines...');
    
    const allWorkspaces = await storage.getAllWorkspaces();
    
    for (const workspace of allWorkspaces) {
      const tasks = await storage.getTasksByWorkspace(workspace.id);
      
      const incompleteTasks = tasks.filter(t => !t.isCompleted && t.dueDate);
      
      for (const task of incompleteTasks) {
        const dueDate = new Date(task.dueDate!);
        const daysDiff = getDaysDifference(now, dueDate);
        
        const project = task.projectId ? await storage.getProjectById(task.projectId, workspace.id) : null;
        
        const taskData = {
          id: task.id,
          title: task.title,
          dueDate: task.dueDate,
          projectId: task.projectId,
          projectTitle: project?.title,
          assigneeId: task.assigneeId,
        };
        
        if (daysDiff === 1) {
          console.log(`[TaskReminder] Task "${task.title}" is due tomorrow`);
          await workflowEngine.fireTrigger('TASK_DUE_SOON', {
            workspaceId: workspace.id,
            entityType: 'task',
            entityId: task.id,
            entityData: {
              ...taskData,
              daysUntilDue: 1,
              urgency: 'HIGH',
            },
          });
        } else if (daysDiff === 3) {
          console.log(`[TaskReminder] Task "${task.title}" is due in 3 days`);
          await workflowEngine.fireTrigger('TASK_DUE_SOON', {
            workspaceId: workspace.id,
            entityType: 'task',
            entityId: task.id,
            entityData: {
              ...taskData,
              daysUntilDue: 3,
              urgency: 'MEDIUM',
            },
          });
        } else if (daysDiff === 7) {
          console.log(`[TaskReminder] Task "${task.title}" is due in 7 days`);
          await workflowEngine.fireTrigger('TASK_DUE_SOON', {
            workspaceId: workspace.id,
            entityType: 'task',
            entityId: task.id,
            entityData: {
              ...taskData,
              daysUntilDue: 7,
              urgency: 'LOW',
            },
          });
        } else if (daysDiff < 0) {
          console.log(`[TaskReminder] Task "${task.title}" is ${Math.abs(daysDiff)} days overdue`);
          await workflowEngine.fireTrigger('TASK_OVERDUE', {
            workspaceId: workspace.id,
            entityType: 'task',
            entityId: task.id,
            entityData: {
              ...taskData,
              daysOverdue: Math.abs(daysDiff),
            },
          });
        }
      }
    }
    
    console.log('[TaskReminder] Task deadline check complete');
  } catch (error) {
    console.error('[TaskReminder] Error checking task deadlines:', error);
  }
}

export function startTaskReminderJob() {
  console.log('[TaskReminder] Starting task reminder job...');
  checkTaskDeadlines();
  setInterval(checkTaskDeadlines, CHECK_INTERVAL);
}

export { checkTaskDeadlines };
