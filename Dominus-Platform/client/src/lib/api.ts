// API client for backend communication
const API_BASE = '/api';

interface RegisterData {
  email: string;
  password: string;
  name: string;
  workspaceName: string;
  workspaceSlug: string;
}

interface LoginData {
  email: string;
  password: string;
}

class ApiError extends Error {
  needsAuth?: boolean;
  constructor(message: string, fields?: { needsAuth?: boolean }) {
    super(message);
    this.needsAuth = fields?.needsAuth;
  }
}

async function handleResponse(response: Response) {
  if (!response.ok) {
    const error = await response.json().catch(() => ({ message: 'Request failed' }));
    const apiError = new ApiError(error.message || `HTTP error! status: ${response.status}`, {
      needsAuth: error.needsAuth,
    });
    throw apiError;
  }
  return response.json();
}

export const api = {
  // Auth
  register: async (data: RegisterData) => {
    const res = await fetch(`${API_BASE}/auth/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  login: async (data: LoginData) => {
    const res = await fetch(`${API_BASE}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  logout: async () => {
    const res = await fetch(`${API_BASE}/auth/logout`, {
      method: 'POST',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  me: async () => {
    const res = await fetch(`${API_BASE}/auth/me`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Clients
  getClients: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getClient: async (workspaceId: string, clientId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createClient: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateClient: async (workspaceId: string, clientId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteClient: async (workspaceId: string, clientId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Client Contacts
  getClientContacts: async (workspaceId: string, clientId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/contacts`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createClientContact: async (workspaceId: string, clientId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/contacts`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateClientContact: async (workspaceId: string, clientId: string, contactId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/contacts/${contactId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteClientContact: async (workspaceId: string, clientId: string, contactId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/contacts/${contactId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  setPrimaryContact: async (workspaceId: string, clientId: string, contactId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/contacts/${contactId}/set-primary`, {
      method: 'POST',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Client Messages
  getClientMessages: async (workspaceId: string, clientId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/messages`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createClientMessage: async (workspaceId: string, clientId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/messages`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteClientMessage: async (workspaceId: string, clientId: string, messageId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/messages/${messageId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Client 360 View
  getClient360: async (workspaceId: string, clientId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/360`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Projects
  getProjects: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createProject: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateProject: async (workspaceId: string, projectId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteProject: async (workspaceId: string, projectId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  reorderProjects: async (workspaceId: string, projectIds: string[], status: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/reorder`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ projectIds, status }),
    });
    return handleResponse(res);
  },

  // Project Responsibles
  getProjectResponsibles: async (workspaceId: string, projectId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}/responsibles`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  addProjectResponsible: async (workspaceId: string, projectId: string, memberId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}/responsibles`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ memberId }),
    });
    return handleResponse(res);
  },

  removeProjectResponsible: async (workspaceId: string, projectId: string, memberId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}/responsibles/${memberId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Project Followers
  getProjectFollowers: async (workspaceId: string, projectId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}/followers`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  addProjectFollower: async (workspaceId: string, projectId: string, memberId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}/followers`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ memberId }),
    });
    return handleResponse(res);
  },

  removeProjectFollower: async (workspaceId: string, projectId: string, memberId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}/followers/${memberId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Project Communications
  getProjectCommunications: async (workspaceId: string, projectId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}/communications`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createProjectCommunication: async (workspaceId: string, projectId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}/communications`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateProjectCommunication: async (workspaceId: string, projectId: string, commId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}/communications/${commId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteProjectCommunication: async (workspaceId: string, projectId: string, commId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/projects/${projectId}/communications/${commId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Tasks
  getTasks: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getTask: async (workspaceId: string, taskId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks/${taskId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createTask: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  toggleTask: async (workspaceId: string, taskId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks/${taskId}/toggle`, {
      method: 'POST',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  updateTask: async (workspaceId: string, taskId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks/${taskId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteTask: async (workspaceId: string, taskId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks/${taskId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  reorderTasks: async (workspaceId: string, taskIds: string[], isCompleted: boolean) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks/reorder`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ taskIds, isCompleted }),
    });
    return handleResponse(res);
  },

  // Task Comments
  getTaskComments: async (workspaceId: string, taskId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks/${taskId}/comments`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createTaskComment: async (workspaceId: string, taskId: string, content: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks/${taskId}/comments`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ content }),
    });
    return handleResponse(res);
  },

  deleteTaskComment: async (workspaceId: string, taskId: string, commentId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks/${taskId}/comments/${commentId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Time Entries
  getTimeEntries: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/time-entries`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createTimeEntry: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/time-entries`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateTimeEntry: async (workspaceId: string, entryId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/time-entries/${entryId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteTimeEntry: async (workspaceId: string, entryId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/time-entries/${entryId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Invoices
  getInvoices: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/invoices`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createInvoice: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/invoices`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateInvoice: async (workspaceId: string, invoiceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/invoices/${invoiceId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  getInvoiceItems: async (workspaceId: string, invoiceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/invoices/${invoiceId}/items`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  deleteInvoice: async (workspaceId: string, invoiceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/invoices/${invoiceId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Proposals
  getProposals: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposals`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getProposal: async (workspaceId: string, proposalId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposals/${proposalId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createProposal: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposals`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateProposal: async (workspaceId: string, proposalId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposals/${proposalId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteProposal: async (workspaceId: string, proposalId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposals/${proposalId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getProposalItems: async (workspaceId: string, proposalId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposals/${proposalId}/items`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getProposalTemplates: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposal-templates`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  applyProposalTemplate: async (workspaceId: string, proposalId: string, templateId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposals/${proposalId}/apply-template`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ templateId }),
    });
    return handleResponse(res);
  },

  generateProposalLink: async (workspaceId: string, proposalId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposals/${proposalId}/generate-link`, {
      method: 'POST',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  generateContractFromProposal: async (workspaceId: string, proposalId: string, data: {
    title: string;
    content?: string;
    templateId?: string | null;
    projectId?: string | null;
    totalValue: number;
    startsOn: string;
    endsOn: string;
    scheduleType: 'EQUAL' | 'PERCENTAGE' | 'MANUAL';
    installmentCount?: number;
    installmentFrequency?: string;
    installmentDueDay?: number;
    installments?: Array<{
      sequence: number;
      amount: number;
      percentage?: number;
      dueDate: string;
      offsetDays?: number;
      description?: string;
    }>;
    generateTransactions?: boolean;
    categoryId?: string | null;
  }) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposals/${proposalId}/generate-contract`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  // Public Proposal (no auth)
  getPublicProposal: async (token: string) => {
    const res = await fetch(`${API_BASE}/public/proposals/${token}`);
    return handleResponse(res);
  },

  getPublicProposalVisual: async (token: string) => {
    const res = await fetch(`${API_BASE}/public/proposals/${token}/visual`);
    return handleResponse(res);
  },

  signProposal: async (token: string, data: { signedByName: string; signatureData: string }) => {
    const res = await fetch(`${API_BASE}/public/proposals/${token}/sign`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  // Financial Categories
  getFinancialCategories: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/financial-categories`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createFinancialCategory: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/financial-categories`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateFinancialCategory: async (workspaceId: string, categoryId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/financial-categories/${categoryId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteFinancialCategory: async (workspaceId: string, categoryId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/financial-categories/${categoryId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Transactions
  getTransactions: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/transactions`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getTransaction: async (workspaceId: string, transactionId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/transactions/${transactionId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createTransaction: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/transactions`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateTransaction: async (workspaceId: string, transactionId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/transactions/${transactionId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteTransaction: async (workspaceId: string, transactionId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/transactions/${transactionId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createRecurringTransactions: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/transactions/recurring`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  // Transaction Attachments
  getTransactionAttachments: async (workspaceId: string, transactionId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/transactions/${transactionId}/attachments`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createTransactionAttachment: async (workspaceId: string, transactionId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/transactions/${transactionId}/attachments`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteTransactionAttachment: async (workspaceId: string, transactionId: string, attachmentId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/transactions/${transactionId}/attachments/${attachmentId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Budgets
  getBudgets: async (workspaceId: string, year?: number) => {
    const url = year 
      ? `${API_BASE}/workspaces/${workspaceId}/budgets?year=${year}`
      : `${API_BASE}/workspaces/${workspaceId}/budgets`;
    const res = await fetch(url, { credentials: 'include' });
    return handleResponse(res);
  },

  createBudget: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/budgets`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateBudget: async (workspaceId: string, budgetId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/budgets/${budgetId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteBudget: async (workspaceId: string, budgetId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/budgets/${budgetId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Products
  getProducts: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/products`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getProduct: async (workspaceId: string, productId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/products/${productId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createProduct: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/products`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateProduct: async (workspaceId: string, productId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/products/${productId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteProduct: async (workspaceId: string, productId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/products/${productId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Contract Templates
  getContractTemplates: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contract-templates`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getContractTemplate: async (workspaceId: string, templateId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contract-templates/${templateId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createContractTemplate: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contract-templates`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateContractTemplate: async (workspaceId: string, templateId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contract-templates/${templateId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteContractTemplate: async (workspaceId: string, templateId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contract-templates/${templateId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Contracts
  getContracts: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contracts`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getContract: async (workspaceId: string, contractId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contracts/${contractId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createContract: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contracts`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateContract: async (workspaceId: string, contractId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contracts/${contractId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteContract: async (workspaceId: string, contractId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contracts/${contractId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  generateContractLink: async (workspaceId: string, contractId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/contracts/${contractId}/generate-link`, {
      method: 'POST',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Public Contract (no auth)
  getPublicContract: async (token: string) => {
    const res = await fetch(`${API_BASE}/public/contracts/${token}`);
    return handleResponse(res);
  },

  signContract: async (token: string, data: { signedByName: string; signatureData: string }) => {
    const res = await fetch(`${API_BASE}/public/contracts/${token}/sign`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  // Lead Workflows
  getLeadWorkflows: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-workflows`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createLeadWorkflow: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-workflows`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateLeadWorkflow: async (workspaceId: string, workflowId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-workflows/${workflowId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteLeadWorkflow: async (workspaceId: string, workflowId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-workflows/${workflowId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Lead Workflow Stages
  getLeadWorkflowStages: async (workspaceId: string, workflowId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-workflows/${workflowId}/stages`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createLeadWorkflowStage: async (workspaceId: string, workflowId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-workflows/${workflowId}/stages`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateLeadWorkflowStage: async (workspaceId: string, workflowId: string, stageId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-workflows/${workflowId}/stages/${stageId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteLeadWorkflowStage: async (workspaceId: string, workflowId: string, stageId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-workflows/${workflowId}/stages/${stageId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  reorderLeadWorkflowStages: async (workspaceId: string, workflowId: string, stageIds: string[]) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-workflows/${workflowId}/stages/reorder`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ stageIds }),
    });
    return handleResponse(res);
  },

  // Leads
  getLeads: async (workspaceId: string, filters?: { workflowId?: string; stageId?: string }) => {
    let url = `${API_BASE}/workspaces/${workspaceId}/leads`;
    const params = new URLSearchParams();
    if (filters?.workflowId) params.append('workflowId', filters.workflowId);
    if (filters?.stageId) params.append('stageId', filters.stageId);
    if (params.toString()) url += `?${params.toString()}`;
    const res = await fetch(url, { credentials: 'include' });
    return handleResponse(res);
  },

  getLeadsPaginated: async (workspaceId: string, options: {
    page?: number;
    pageSize?: number;
    search?: string;
    source?: string;
    status?: string;
    tagIds?: string[];
    workflowId?: string;
    stageId?: string;
    sortBy?: string;
  } = {}) => {
    const params = new URLSearchParams();
    if (options.page) params.append('page', options.page.toString());
    if (options.pageSize) params.append('pageSize', options.pageSize.toString());
    if (options.search) params.append('search', options.search);
    if (options.source) params.append('source', options.source);
    if (options.status) params.append('status', options.status);
    if (options.workflowId) params.append('workflowId', options.workflowId);
    if (options.stageId) params.append('stageId', options.stageId);
    if (options.tagIds && options.tagIds.length > 0) params.append('tagIds', options.tagIds.join(','));
    if (options.sortBy) params.append('sortBy', options.sortBy);
    const url = `${API_BASE}/workspaces/${workspaceId}/leads?${params.toString()}`;
    const res = await fetch(url, { credentials: 'include' });
    return handleResponse(res);
  },

  // Lead Cleanup
  getLeadsWithoutContact: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads-cleanup/without-contact`, { credentials: 'include' });
    return handleResponse(res);
  },

  getDuplicateLeads: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads-cleanup/duplicates`, { credentials: 'include' });
    return handleResponse(res);
  },

  deleteLeadsBatch: async (workspaceId: string, leadIds: string[]) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads-cleanup/delete-batch`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ leadIds }),
    });
    return handleResponse(res);
  },

  // Lead Tags
  getLeadTags: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-tags`, { credentials: 'include' });
    return handleResponse(res);
  },

  createLeadTag: async (workspaceId: string, data: { name: string; color?: string }) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-tags`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateLeadTag: async (workspaceId: string, tagId: string, data: { name?: string; color?: string }) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-tags/${tagId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteLeadTag: async (workspaceId: string, tagId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-tags/${tagId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  setLeadTags: async (workspaceId: string, leadId: string, tagIds: string[]) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads/${leadId}/tags`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ tagIds }),
    });
    return handleResponse(res);
  },

  createLead: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateLead: async (workspaceId: string, leadId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads/${leadId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  moveLeadStage: async (workspaceId: string, leadId: string, toStageId: string, notes?: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads/${leadId}/move-stage`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ toStageId, notes }),
    });
    return handleResponse(res);
  },

  deleteLead: async (workspaceId: string, leadId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads/${leadId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  importLeads: async (workspaceId: string, leads: any[]) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads/import`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ leads }),
    });
    return handleResponse(res);
  },

  exportLeads: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads/export`, {
      credentials: 'include',
    });
    if (!res.ok) throw new Error('Export failed');
    return res.blob();
  },

  importGoogleContacts: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/leads/import-google-contacts`, {
      method: 'POST',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getGoogleContactsStatus: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/google-contacts/status`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getGoogleContactsAuthUrl: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/google-contacts/auth-url`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  disconnectGoogleContacts: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/google-contacts/disconnect`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Google Calendar OAuth
  getGoogleCalendarStatus: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/google-calendar/status`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getGoogleCalendarAuthUrl: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/google-calendar/auth-url`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  disconnectGoogleCalendar: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/google-calendar/disconnect`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Lead Forms
  getLeadForms: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-forms`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getLeadForm: async (workspaceId: string, formId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-forms/${formId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createLeadForm: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-forms`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateLeadForm: async (workspaceId: string, formId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-forms/${formId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteLeadForm: async (workspaceId: string, formId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-forms/${formId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Lead Form Fields
  createLeadFormField: async (workspaceId: string, formId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-forms/${formId}/fields`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateLeadFormField: async (workspaceId: string, formId: string, fieldId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-forms/${formId}/fields/${fieldId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteLeadFormField: async (workspaceId: string, formId: string, fieldId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-forms/${formId}/fields/${fieldId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  reorderLeadFormFields: async (workspaceId: string, formId: string, fieldIds: string[]) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-forms/${formId}/fields/reorder`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ fieldIds }),
    });
    return handleResponse(res);
  },

  // Lead Landing Pages
  getLeadLandingPages: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-landing-pages`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createLeadLandingPage: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-landing-pages`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateLeadLandingPage: async (workspaceId: string, pageId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-landing-pages/${pageId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteLeadLandingPage: async (workspaceId: string, pageId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/lead-landing-pages/${pageId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Global Search
  globalSearch: async (workspaceId: string, query: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/search?q=${encodeURIComponent(query)}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Dashboard
  getDashboardStats: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/dashboard/stats`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getCashflowData: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/dashboard/cashflow`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getIncomeByCategory: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/dashboard/income-by-category`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getCashflowForecast: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/dashboard/cashflow-forecast`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Custom Fields
  getCustomFields: async (workspaceId: string, entityType?: string) => {
    const url = entityType 
      ? `${API_BASE}/workspaces/${workspaceId}/custom-fields?entityType=${entityType}`
      : `${API_BASE}/workspaces/${workspaceId}/custom-fields`;
    const res = await fetch(url, { credentials: 'include' });
    return handleResponse(res);
  },

  createCustomField: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/custom-fields`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateCustomField: async (workspaceId: string, fieldId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/custom-fields/${fieldId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteCustomField: async (workspaceId: string, fieldId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/custom-fields/${fieldId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  reorderCustomFields: async (workspaceId: string, entityType: string, definitionIds: string[]) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/custom-fields/reorder`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ entityType, definitionIds }),
    });
    return handleResponse(res);
  },

  getCustomFieldValues: async (workspaceId: string, entityType: string, entityId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/custom-field-values/${entityType}/${entityId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  saveCustomFieldValues: async (workspaceId: string, entityType: string, entityId: string, values: any[]) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/custom-field-values/${entityType}/${entityId}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ values }),
    });
    return handleResponse(res);
  },

  // Profile
  getProfile: async () => {
    const res = await fetch(`${API_BASE}/profile`, { credentials: 'include' });
    return handleResponse(res);
  },

  updateProfile: async (data: { name?: string; email?: string; avatar?: string }) => {
    const res = await fetch(`${API_BASE}/profile`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  changePassword: async (currentPassword: string, newPassword: string) => {
    const res = await fetch(`${API_BASE}/profile/change-password`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ currentPassword, newPassword }),
    });
    return handleResponse(res);
  },

  // Team
  getTeamMembers: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/team`, { credentials: 'include' });
    return handleResponse(res);
  },

  updateMemberRole: async (workspaceId: string, memberId: string, memberRole: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/team/${memberId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ memberRole }),
    });
    return handleResponse(res);
  },

  removeMember: async (workspaceId: string, memberId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/team/${memberId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getMemberPermissions: async (workspaceId: string, memberId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/team/${memberId}/permissions`, { credentials: 'include' });
    return handleResponse(res);
  },

  setMemberPermission: async (workspaceId: string, memberId: string, module: string, action: string, granted: boolean) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/team/${memberId}/permissions`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ module, action, granted }),
    });
    return handleResponse(res);
  },

  checkPermission: async (workspaceId: string, module: string, action: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/check-permission?module=${module}&action=${action}`, { credentials: 'include' });
    return handleResponse(res);
  },

  // Workspace Settings
  updateWorkspace: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  // Email Templates
  getEmailTemplates: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/email-templates`, { credentials: 'include' });
    return handleResponse(res);
  },

  getEmailTemplate: async (workspaceId: string, id: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/email-templates/${id}`, { credentials: 'include' });
    return handleResponse(res);
  },

  createEmailTemplate: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/email-templates`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateEmailTemplate: async (workspaceId: string, id: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/email-templates/${id}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteEmailTemplate: async (workspaceId: string, id: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/email-templates/${id}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // SMS Templates
  getSmsTemplates: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sms-templates`, { credentials: 'include' });
    return handleResponse(res);
  },

  getSmsTemplate: async (workspaceId: string, id: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sms-templates/${id}`, { credentials: 'include' });
    return handleResponse(res);
  },

  createSmsTemplate: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sms-templates`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateSmsTemplate: async (workspaceId: string, id: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sms-templates/${id}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteSmsTemplate: async (workspaceId: string, id: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sms-templates/${id}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getSmsSettings: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sms-settings`, { credentials: 'include' });
    return handleResponse(res);
  },

  updateSmsSettings: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sms-settings`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  getSmsLogs: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sms-logs`, { credentials: 'include' });
    return handleResponse(res);
  },

  getSmsUsage: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sms-usage`, { credentials: 'include' });
    return handleResponse(res);
  },

  // WhatsApp Templates
  getWhatsappTemplates: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/whatsapp-templates`, { credentials: 'include' });
    return handleResponse(res);
  },

  getWhatsappTemplate: async (workspaceId: string, id: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/whatsapp-templates/${id}`, { credentials: 'include' });
    return handleResponse(res);
  },

  createWhatsappTemplate: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/whatsapp-templates`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateWhatsappTemplate: async (workspaceId: string, id: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/whatsapp-templates/${id}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteWhatsappTemplate: async (workspaceId: string, id: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/whatsapp-templates/${id}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getWhatsappSettings: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/whatsapp-settings`, { credentials: 'include' });
    return handleResponse(res);
  },

  updateWhatsappSettings: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/whatsapp-settings`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  getWhatsappLogs: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/whatsapp-logs`, { credentials: 'include' });
    return handleResponse(res);
  },

  getWhatsappUsage: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/whatsapp-usage`, { credentials: 'include' });
    return handleResponse(res);
  },

  // Invites
  getWorkspaceInvites: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/invites`, { credentials: 'include' });
    return handleResponse(res);
  },

  createInvite: async (workspaceId: string, email: string, memberRole: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/invites`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ email, memberRole }),
    });
    return handleResponse(res);
  },

  cancelInvite: async (workspaceId: string, inviteId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/invites/${inviteId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getInviteDetails: async (token: string) => {
    const res = await fetch(`${API_BASE}/invites/${token}`);
    return handleResponse(res);
  },

  acceptInvite: async (token: string) => {
    const res = await fetch(`${API_BASE}/invites/${token}/accept`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // Schedulers
  getSchedulers: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/schedulers`, { credentials: 'include' });
    return handleResponse(res);
  },

  createScheduler: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/schedulers`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  getScheduler: async (workspaceId: string, schedulerId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/schedulers/${schedulerId}`, { credentials: 'include' });
    return handleResponse(res);
  },

  updateScheduler: async (workspaceId: string, schedulerId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/schedulers/${schedulerId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteScheduler: async (workspaceId: string, schedulerId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/schedulers/${schedulerId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    if (!res.ok) {
      const error = await res.json().catch(() => ({ message: 'Request failed' }));
      throw new Error(error.message);
    }
  },

  updateSchedulerAvailability: async (workspaceId: string, schedulerId: string, availability: any[]) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/schedulers/${schedulerId}/availability`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ availability }),
    });
    return handleResponse(res);
  },

  createSchedulerException: async (workspaceId: string, schedulerId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/schedulers/${schedulerId}/exceptions`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteSchedulerException: async (workspaceId: string, schedulerId: string, exceptionId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/schedulers/${schedulerId}/exceptions/${exceptionId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    if (!res.ok) {
      const error = await res.json().catch(() => ({ message: 'Request failed' }));
      throw new Error(error.message);
    }
  },

  // Bookings
  getBookings: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/bookings`, { credentials: 'include' });
    return handleResponse(res);
  },

  updateBooking: async (workspaceId: string, bookingId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/bookings/${bookingId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteBooking: async (workspaceId: string, bookingId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/bookings/${bookingId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    if (!res.ok) {
      const error = await res.json().catch(() => ({ message: 'Request failed' }));
      throw new Error(error.message);
    }
  },

  // Public scheduler endpoints
  getPublicScheduler: async (workspaceSlug: string, schedulerSlug: string) => {
    const res = await fetch(`${API_BASE}/public/schedule/${workspaceSlug}/${schedulerSlug}`);
    return handleResponse(res);
  },

  getPublicSchedulerSlots: async (workspaceSlug: string, schedulerSlug: string, date: string) => {
    const res = await fetch(`${API_BASE}/public/schedule/${workspaceSlug}/${schedulerSlug}/slots?date=${date}`);
    return handleResponse(res);
  },

  createPublicBooking: async (workspaceSlug: string, schedulerSlug: string, data: any) => {
    const res = await fetch(`${API_BASE}/public/schedule/${workspaceSlug}/${schedulerSlug}/book`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  // ============ SITE BUILDER ============

  // Site Projects
  getSiteProjects: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sites`, { credentials: 'include' });
    return handleResponse(res);
  },

  getSiteProject: async (workspaceId: string, siteId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sites/${siteId}`, { credentials: 'include' });
    return handleResponse(res);
  },

  createSiteProject: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sites`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateSiteProject: async (workspaceId: string, siteId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sites/${siteId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteSiteProject: async (workspaceId: string, siteId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/sites/${siteId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    if (!res.ok) throw new Error('Failed to delete site');
  },

  // Site Pages
  getSitePages: async (siteId: string) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/pages`, { credentials: 'include' });
    return handleResponse(res);
  },

  getSitePage: async (siteId: string, pageId: string) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/pages/${pageId}`, { credentials: 'include' });
    return handleResponse(res);
  },

  createSitePage: async (siteId: string, data: any) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/pages`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateSitePage: async (siteId: string, pageId: string, data: any) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/pages/${pageId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteSitePage: async (siteId: string, pageId: string) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/pages/${pageId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    if (!res.ok) throw new Error('Failed to delete page');
  },

  // Blog Categories
  getBlogCategories: async (siteId: string) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/blog/categories`, { credentials: 'include' });
    return handleResponse(res);
  },

  createBlogCategory: async (siteId: string, data: any) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/blog/categories`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateBlogCategory: async (siteId: string, categoryId: string, data: any) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/blog/categories/${categoryId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteBlogCategory: async (siteId: string, categoryId: string) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/blog/categories/${categoryId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    if (!res.ok) throw new Error('Failed to delete category');
  },

  // Blog Posts
  getBlogPosts: async (siteId: string) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/blog/posts`, { credentials: 'include' });
    return handleResponse(res);
  },

  getBlogPost: async (siteId: string, postId: string) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/blog/posts/${postId}`, { credentials: 'include' });
    return handleResponse(res);
  },

  createBlogPost: async (siteId: string, data: any) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/blog/posts`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateBlogPost: async (siteId: string, postId: string, data: any) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/blog/posts/${postId}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteBlogPost: async (siteId: string, postId: string) => {
    const res = await fetch(`${API_BASE}/sites/${siteId}/blog/posts/${postId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    if (!res.ok) throw new Error('Failed to delete post');
  },

  // ============ COMMENTS (GENERIC) ============
  getComments: async (workspaceId: string, entityType: string, entityId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/comments/${entityType}/${entityId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createComment: async (workspaceId: string, entityType: string, entityId: string, data: { content: string; parentId?: string }) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/comments/${entityType}/${entityId}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateComment: async (workspaceId: string, commentId: string, content: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/comments/${commentId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ content }),
    });
    return handleResponse(res);
  },

  deleteComment: async (workspaceId: string, commentId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/comments/${commentId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // ============ FILES ============
  getFiles: async (workspaceId: string, filters?: { projectId?: string; clientId?: string; taskId?: string; folder?: string }) => {
    const params = new URLSearchParams();
    if (filters?.projectId) params.append('projectId', filters.projectId);
    if (filters?.clientId) params.append('clientId', filters.clientId);
    if (filters?.taskId) params.append('taskId', filters.taskId);
    if (filters?.folder) params.append('folder', filters.folder);
    const queryString = params.toString();
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/files${queryString ? `?${queryString}` : ''}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getFile: async (workspaceId: string, fileId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/files/${fileId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getFileUploadUrl: async (workspaceId: string, data: { name: string; size: number; contentType: string }) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/files/request-url`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  createFile: async (workspaceId: string, data: {
    fileName: string;
    originalName: string;
    fileUrl: string;
    fileSize: number;
    mimeType?: string;
    folder?: string;
    projectId?: string;
    clientId?: string;
    taskId?: string;
    isPublic?: boolean;
  }) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/files`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteFile: async (workspaceId: string, fileId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/files/${fileId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // ============ CLIENT DOCUMENTS ============
  getClientDocuments: async (workspaceId: string, clientId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/documents`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getExpiringDocuments: async (workspaceId: string, days: number = 30) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/documents/expiring?days=${days}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getClientDocumentUploadUrl: async (workspaceId: string, clientId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/documents/request-url`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createClientDocument: async (workspaceId: string, clientId: string, data: {
    name: string;
    description?: string;
    documentType: string;
    documentNumber?: string;
    fileName: string;
    fileUrl: string;
    fileSize?: number;
    mimeType?: string;
    issueDate?: string;
    expirationDate?: string;
    alertDaysBefore?: number;
    notes?: string;
  }) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/clients/${clientId}/documents`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateClientDocument: async (workspaceId: string, documentId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/documents/${documentId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteClientDocument: async (workspaceId: string, documentId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/documents/${documentId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // ============ WORKFLOWS (AUTOMATION) ============
  getWorkflows: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/workflows`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getWorkflow: async (workspaceId: string, workflowId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/workflows/${workflowId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getWorkflowTemplates: async () => {
    const res = await fetch(`${API_BASE}/workflows/templates`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createWorkflow: async (workspaceId: string, data: {
    name: string;
    description?: string;
    triggerType: string;
    triggerConditions?: string;
    actions: any[];
    status?: string;
  }) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/workflows`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  updateWorkflow: async (workspaceId: string, workflowId: string, data: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/workflows/${workflowId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  deleteWorkflow: async (workspaceId: string, workflowId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/workflows/${workflowId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  toggleWorkflow: async (workspaceId: string, workflowId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/workflows/${workflowId}/toggle`, {
      method: 'POST',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getWorkflowExecutions: async (workspaceId: string, workflowId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/workflows/${workflowId}/executions`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  testWorkflow: async (workspaceId: string, workflowId: string, testData?: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/workflows/${workflowId}/test`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ testData }),
    });
    return handleResponse(res);
  },

  // ============ AI CHAT (SUPER WORK AI) ============
  getAiSessions: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/ai/sessions`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  createAiSession: async (workspaceId: string, data?: { title?: string; contextType?: string; contextId?: string }) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/ai/sessions`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data || {}),
    });
    return handleResponse(res);
  },

  getAiSession: async (workspaceId: string, sessionId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/ai/sessions/${sessionId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  sendAiMessage: async (workspaceId: string, sessionId: string, message: string, contextData?: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/ai/sessions/${sessionId}/messages`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ message, contextData }),
    });
    return handleResponse(res);
  },

  deleteAiSession: async (workspaceId: string, sessionId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/ai/sessions/${sessionId}`, {
      method: 'DELETE',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  generateAiContent: async (workspaceId: string, prompt: string, type?: string, context?: any) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/ai/generate`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ prompt, type, context }),
    });
    return handleResponse(res);
  },

  // ============ NOTIFICATIONS ============
  getNotifications: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/notifications`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getUnreadNotificationCount: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/notifications/unread-count`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  markNotificationRead: async (workspaceId: string, notificationId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/notifications/${notificationId}/read`, {
      method: 'POST',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  markAllNotificationsRead: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/notifications/read-all`, {
      method: 'POST',
      credentials: 'include',
    });
    return handleResponse(res);
  },

  // ============ HOST PANEL ============
  checkHostAccess: async () => {
    const res = await fetch(`${API_BASE}/host/check`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getHostDashboard: async () => {
    const res = await fetch(`${API_BASE}/host/dashboard`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getHostWorkspaces: async () => {
    const res = await fetch(`${API_BASE}/host/workspaces`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getHostWorkspace: async (workspaceId: string) => {
    const res = await fetch(`${API_BASE}/host/workspaces/${workspaceId}`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  updateWorkspaceSubscription: async (workspaceId: string, data: any) => {
    const res = await fetch(`${API_BASE}/host/workspaces/${workspaceId}/subscription`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data),
    });
    return handleResponse(res);
  },

  getHostUsers: async () => {
    const res = await fetch(`${API_BASE}/host/users`, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  updateUserGlobalRole: async (userId: string, role: 'USER' | 'HOST') => {
    const res = await fetch(`${API_BASE}/host/users/${userId}/role`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ role }),
    });
    return handleResponse(res);
  },

  getHostEvents: async (limit?: number) => {
    const url = limit ? `${API_BASE}/host/events?limit=${limit}` : `${API_BASE}/host/events`;
    const res = await fetch(url, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  getWorkspaceEvents: async (workspaceId: string, limit?: number) => {
    const url = limit ? `${API_BASE}/host/workspaces/${workspaceId}/events?limit=${limit}` : `${API_BASE}/host/workspaces/${workspaceId}/events`;
    const res = await fetch(url, {
      credentials: 'include',
    });
    return handleResponse(res);
  },

  parseTasksFromText: async (workspaceId: string, text: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks/parse-from-text`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ text }),
    });
    return handleResponse(res);
  },

  batchCreateTasks: async (workspaceId: string, tasks: any[], projectId?: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/tasks/batch-create`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ tasks, projectId }),
    });
    return handleResponse(res);
  },

  analyzeProposalText: async (workspaceId: string, text: string) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposal-copilot/analyze`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ text }),
    });
    return handleResponse(res);
  },

  searchClientsForCopilot: async (workspaceId: string, terms: string[]) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposal-copilot/search-clients`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ terms }),
    });
    return handleResponse(res);
  },

  searchProductsForCopilot: async (workspaceId: string, terms: string[]) => {
    const res = await fetch(`${API_BASE}/workspaces/${workspaceId}/proposal-copilot/search-products`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ terms }),
    });
    return handleResponse(res);
  },
};
