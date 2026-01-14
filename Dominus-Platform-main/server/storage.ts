import { drizzle } from "drizzle-orm/node-postgres";
import pg from "pg";
import * as schema from "@shared/schema";
import { eq, and, desc, asc, gte, lte, sql, or, ilike, inArray, isNull } from "drizzle-orm";
import type {
  User,
  InsertUser,
  Workspace,
  InsertWorkspace,
  WorkspaceMember,
  InsertWorkspaceMember,
  MemberPermission,
  InsertMemberPermission,
  WorkspaceInvite,
  InsertWorkspaceInvite,
  Client,
  InsertClient,
  ClientContact,
  InsertClientContact,
  ClientMessage,
  InsertClientMessage,
  Project,
  InsertProject,
  Task,
  InsertTask,
  Invoice,
  InsertInvoice,
  InvoiceItem,
  InsertInvoiceItem,
  Proposal,
  InsertProposal,
  ProposalItem,
  InsertProposalItem,
  TimeEntry,
  InsertTimeEntry,
  TaskComment,
  InsertTaskComment,
  FinancialCategory,
  InsertFinancialCategory,
  Transaction,
  InsertTransaction,
  TransactionAttachment,
  InsertTransactionAttachment,
  Budget,
  InsertBudget,
  Product,
  InsertProduct,
  ContractTemplate,
  InsertContractTemplate,
  Contract,
  InsertContract,
  ContractItem,
  InsertContractItem,
  ContractInstallment,
  InsertContractInstallment,
  LeadWorkflow,
  InsertLeadWorkflow,
  LeadWorkflowStage,
  InsertLeadWorkflowStage,
  LeadForm,
  InsertLeadForm,
  LeadFormField,
  InsertLeadFormField,
  LeadLandingPage,
  InsertLeadLandingPage,
  Lead,
  InsertLead,
  LeadTag,
  InsertLeadTag,
  LeadTagAssignment,
  InsertLeadTagAssignment,
  LeadFormSubmission,
  InsertLeadFormSubmission,
  LeadStageHistory,
  InsertLeadStageHistory,
  CustomFieldDefinition,
  InsertCustomFieldDefinition,
  CustomFieldValue,
  InsertCustomFieldValue,
  SchedulerType,
  InsertSchedulerType,
  SchedulerAvailability,
  InsertSchedulerAvailability,
  SchedulerException,
  InsertSchedulerException,
  SchedulerBooking,
  InsertSchedulerBooking,
  PermissionModule,
  PermissionAction,
  MemberRole,
  InsertSiteProject,
  SiteProject,
  InsertSitePage,
  SitePage,
  InsertSitePageVersion,
  SitePageVersion,
  InsertBlogCategory,
  BlogCategory,
  InsertBlogPost,
  BlogPost,
  InsertBlogPostVersion,
  BlogPostVersion,
  InsertSiteVisitEvent,
  SiteVisitEvent,
  SiteVisitDailyStat,
  InsertSiteVisitDailyStat,
  Conversation,
  InsertConversation,
  ConversationParticipant,
  InsertConversationParticipant,
  Message,
  InsertMessage,
  Comment,
  InsertComment,
  File,
  InsertFile,
  ClientDocument,
  InsertClientDocument,
  Notification,
  InsertNotification,
  Workflow,
  InsertWorkflow,
  WorkflowExecution,
  InsertWorkflowExecution,
  AiChatSession,
  InsertAiChatSession,
  AiChatMessage,
  InsertAiChatMessage,
  WorkspaceSubscription,
  InsertWorkspaceSubscription,
  WorkspaceUsageMetrics,
  InsertWorkspaceUsageMetrics,
  WorkspaceAccessEvent,
  InsertWorkspaceAccessEvent,
  GlobalRole,
  LandingLead,
  InsertLandingLead,
  ProposalTemplate,
  InsertProposalTemplate,
  ProposalBlockInstance,
  InsertProposalBlockInstance,
  ProposalTemplateBlock,
  InsertProposalTemplateBlock,
  EmailLog,
  InsertEmailLog,
  EmailTemplate,
  InsertEmailTemplate,
  SmsTemplate,
  InsertSmsTemplate,
  SmsLog,
  InsertSmsLog,
  WhatsappTemplate,
  InsertWhatsappTemplate,
  WhatsappLog,
  InsertWhatsappLog,
  WorkspaceGoogleCredentials,
  InsertWorkspaceGoogleCredentials,
  WorkspaceGoogleCalendarCredentials,
  InsertWorkspaceGoogleCalendarCredentials,
  ProjectResponsible,
  InsertProjectResponsible,
  projectResponsibles,
  ProjectFollower,
  InsertProjectFollower,
  projectFollowers,
  ProjectCommunication,
  InsertProjectCommunication,
  projectCommunications,
} from "@shared/schema";

const pool = new pg.Pool({
  connectionString: process.env.DATABASE_URL,
});

export const db = drizzle(pool, { schema });

// Storage Interface
export interface IStorage {
  // Users
  createUser(user: InsertUser): Promise<User>;
  getUserById(id: string): Promise<User | undefined>;
  getUserByEmail(email: string): Promise<User | undefined>;

  // Workspaces
  createWorkspace(workspace: InsertWorkspace): Promise<Workspace>;
  getWorkspaceBySlug(slug: string): Promise<Workspace | undefined>;
  getWorkspaceById(id: string): Promise<Workspace | undefined>;
  getUserWorkspaces(userId: string): Promise<Workspace[]>;
  getAllWorkspaces(): Promise<Workspace[]>;
  updateWorkspace(workspaceId: string, data: Partial<InsertWorkspace>): Promise<Workspace | undefined>;

  // Workspace Members
  addWorkspaceMember(member: InsertWorkspaceMember): Promise<WorkspaceMember>;
  getWorkspaceMembers(workspaceId: string): Promise<WorkspaceMember[]>;
  getWorkspaceMembersWithUsers(workspaceId: string): Promise<(WorkspaceMember & { user: User })[]>;
  getUserWorkspaceMembership(userId: string, workspaceId: string): Promise<WorkspaceMember | undefined>;
  updateWorkspaceMember(memberId: string, workspaceId: string, data: { memberRole?: MemberRole }): Promise<WorkspaceMember | undefined>;
  removeWorkspaceMember(memberId: string, workspaceId: string): Promise<void>;

  // User Profile
  updateUser(id: string, data: Partial<InsertUser>): Promise<User | undefined>;

  // Member Permissions
  getMemberPermissions(memberId: string, workspaceId: string): Promise<MemberPermission[]>;
  setMemberPermission(data: InsertMemberPermission): Promise<MemberPermission>;
  removeMemberPermission(memberId: string, module: PermissionModule, action: PermissionAction, workspaceId: string): Promise<void>;
  clearMemberPermissions(memberId: string, workspaceId: string): Promise<void>;
  checkPermission(memberId: string, workspaceId: string, module: PermissionModule, action: PermissionAction): Promise<boolean>;

  // Workspace Invites
  createWorkspaceInvite(invite: InsertWorkspaceInvite): Promise<WorkspaceInvite>;
  getWorkspaceInvites(workspaceId: string): Promise<WorkspaceInvite[]>;
  getInviteByToken(token: string): Promise<WorkspaceInvite | undefined>;
  updateInviteStatus(id: string, status: 'ACCEPTED' | 'EXPIRED' | 'CANCELLED'): Promise<WorkspaceInvite | undefined>;
  deleteWorkspaceInvite(id: string, workspaceId: string): Promise<void>;

  // Clients
  createClient(client: InsertClient): Promise<Client>;
  getClientsByWorkspace(workspaceId: string): Promise<Client[]>;
  getClientsByWorkspacePaginated(
    workspaceId: string, 
    options?: { 
      page?: number; 
      pageSize?: number; 
      search?: string;
      clientType?: string;
      sortBy?: string;
    }
  ): Promise<{ items: Client[]; total: number; page: number; pageSize: number }>;
  getClientById(id: string, workspaceId: string): Promise<Client | undefined>;
  updateClient(id: string, workspaceId: string, data: Partial<InsertClient>): Promise<Client | undefined>;
  deleteClient(id: string, workspaceId: string): Promise<void>;

  // Client Contacts
  createClientContact(contact: InsertClientContact): Promise<ClientContact>;
  getClientContacts(clientId: string): Promise<ClientContact[]>;
  updateClientContact(id: string, data: Partial<InsertClientContact>): Promise<ClientContact | undefined>;
  deleteClientContact(id: string): Promise<void>;
  setPrimaryContact(clientId: string, contactId: string): Promise<void>;

  // Client Messages
  createClientMessage(message: InsertClientMessage): Promise<ClientMessage>;
  getClientMessages(clientId: string, workspaceId: string): Promise<ClientMessage[]>;
  updateClientMessage(id: string, workspaceId: string, data: Partial<InsertClientMessage>): Promise<ClientMessage | undefined>;
  deleteClientMessage(id: string, workspaceId: string): Promise<void>;

  // Projects
  createProject(project: InsertProject): Promise<Project>;
  getProjectsByWorkspace(workspaceId: string): Promise<Project[]>;
  getProjectById(id: string, workspaceId: string): Promise<Project | undefined>;
  updateProject(id: string, workspaceId: string, data: Partial<InsertProject>): Promise<Project | undefined>;
  deleteProject(id: string, workspaceId: string): Promise<void>;
  reorderProjects(workspaceId: string, projectIds: string[], status: string): Promise<void>;

  // Tasks
  createTask(task: InsertTask): Promise<Task>;
  getTasksByWorkspace(workspaceId: string): Promise<Task[]>;
  getTasksByProject(projectId: string, workspaceId: string): Promise<Task[]>;
  getTaskById(id: string, workspaceId: string): Promise<Task | undefined>;
  updateTask(id: string, workspaceId: string, data: Partial<InsertTask>): Promise<Task | undefined>;
  deleteTask(id: string, workspaceId: string): Promise<void>;
  toggleTaskComplete(id: string, workspaceId: string): Promise<Task | undefined>;
  reorderTasks(workspaceId: string, taskIds: string[], isCompleted: boolean): Promise<void>;

  // Task Comments
  createTaskComment(comment: InsertTaskComment): Promise<TaskComment>;
  getTaskComments(taskId: string): Promise<TaskComment[]>;
  deleteTaskComment(id: string): Promise<void>;

  // Invoices
  createInvoice(invoice: InsertInvoice): Promise<Invoice>;
  getInvoicesByWorkspace(workspaceId: string): Promise<Invoice[]>;
  getInvoiceById(id: string, workspaceId: string): Promise<Invoice | undefined>;
  updateInvoice(id: string, workspaceId: string, data: Partial<InsertInvoice>): Promise<Invoice | undefined>;
  deleteInvoice(id: string, workspaceId: string): Promise<void>;

  // Invoice Items
  createInvoiceItem(item: InsertInvoiceItem): Promise<InvoiceItem>;
  getInvoiceItems(invoiceId: string): Promise<InvoiceItem[]>;
  deleteInvoiceItems(invoiceId: string): Promise<void>;

  // Proposals
  createProposal(proposal: InsertProposal): Promise<Proposal>;
  getProposalsByWorkspace(workspaceId: string): Promise<Proposal[]>;
  getProposalById(id: string, workspaceId: string): Promise<Proposal | undefined>;
  getProposalByToken(token: string): Promise<Proposal | undefined>;
  updateProposal(id: string, workspaceId: string, data: Partial<InsertProposal>): Promise<Proposal | undefined>;
  deleteProposal(id: string, workspaceId: string): Promise<void>;

  // Proposal Items
  createProposalItem(item: InsertProposalItem): Promise<ProposalItem>;
  getProposalItems(proposalId: string): Promise<ProposalItem[]>;
  deleteProposalItems(proposalId: string): Promise<void>;

  // Time Entries
  createTimeEntry(entry: InsertTimeEntry): Promise<TimeEntry>;
  getTimeEntriesByWorkspace(workspaceId: string): Promise<TimeEntry[]>;
  getTimeEntriesByProject(projectId: string, workspaceId: string): Promise<TimeEntry[]>;
  updateTimeEntry(id: string, workspaceId: string, data: Partial<InsertTimeEntry>): Promise<TimeEntry | undefined>;
  deleteTimeEntry(id: string, workspaceId: string): Promise<void>;

  // Financial Categories
  createFinancialCategory(category: InsertFinancialCategory): Promise<FinancialCategory>;
  getFinancialCategoriesByWorkspace(workspaceId: string): Promise<FinancialCategory[]>;
  getFinancialCategoryById(id: string, workspaceId: string): Promise<FinancialCategory | undefined>;
  updateFinancialCategory(id: string, workspaceId: string, data: Partial<InsertFinancialCategory>): Promise<FinancialCategory | undefined>;
  deleteFinancialCategory(id: string, workspaceId: string): Promise<void>;

  // Transactions
  createTransaction(transaction: InsertTransaction): Promise<Transaction>;
  createTransactionsBatch(transactions: InsertTransaction[]): Promise<Transaction[]>;
  getTransactionsByWorkspace(workspaceId: string): Promise<Transaction[]>;
  getTransactionById(id: string, workspaceId: string): Promise<Transaction | undefined>;
  updateTransaction(id: string, workspaceId: string, data: Partial<InsertTransaction>): Promise<Transaction | undefined>;
  deleteTransaction(id: string, workspaceId: string): Promise<void>;
  getTransactionsByDateRange(workspaceId: string, startDate: Date, endDate: Date): Promise<Transaction[]>;

  // Transaction Attachments
  createTransactionAttachment(attachment: InsertTransactionAttachment): Promise<TransactionAttachment>;
  getTransactionAttachments(transactionId: string): Promise<TransactionAttachment[]>;
  deleteTransactionAttachment(id: string): Promise<void>;

  // Budgets
  createBudget(budget: InsertBudget): Promise<Budget>;
  getBudgetsByWorkspace(workspaceId: string, year?: number): Promise<Budget[]>;
  updateBudget(id: string, data: Partial<InsertBudget>): Promise<Budget | undefined>;
  deleteBudget(id: string): Promise<void>;

  // Products
  createProduct(product: InsertProduct): Promise<Product>;
  getProductsByWorkspace(workspaceId: string): Promise<Product[]>;
  getProductById(id: string, workspaceId: string): Promise<Product | undefined>;
  updateProduct(id: string, workspaceId: string, data: Partial<InsertProduct>): Promise<Product | undefined>;
  deleteProduct(id: string, workspaceId: string): Promise<void>;
  searchProducts(workspaceId: string, searchTerms: string[]): Promise<Product[]>;

  // Copilot Search
  searchClientsFlexible(workspaceId: string, searchTerms: string[]): Promise<Client[]>;

  // Contract Templates
  createContractTemplate(template: InsertContractTemplate): Promise<ContractTemplate>;
  getContractTemplatesByWorkspace(workspaceId: string): Promise<ContractTemplate[]>;
  getContractTemplateById(id: string, workspaceId: string): Promise<ContractTemplate | undefined>;
  updateContractTemplate(id: string, workspaceId: string, data: Partial<InsertContractTemplate>): Promise<ContractTemplate | undefined>;
  deleteContractTemplate(id: string, workspaceId: string): Promise<void>;

  // Contracts
  createContract(contract: InsertContract): Promise<Contract>;
  getContractsByWorkspace(workspaceId: string): Promise<Contract[]>;
  getContractById(id: string, workspaceId: string): Promise<Contract | undefined>;
  getContractByToken(token: string): Promise<Contract | undefined>;
  updateContract(id: string, workspaceId: string, data: Partial<InsertContract>): Promise<Contract | undefined>;
  deleteContract(id: string, workspaceId: string): Promise<void>;

  // Contract Items
  createContractItem(item: InsertContractItem): Promise<ContractItem>;
  createContractItemsBatch(items: InsertContractItem[]): Promise<ContractItem[]>;
  getContractItems(contractId: string): Promise<ContractItem[]>;
  deleteContractItems(contractId: string): Promise<void>;

  // Contract Installments
  createContractInstallment(installment: InsertContractInstallment): Promise<ContractInstallment>;
  createContractInstallmentsBatch(installments: InsertContractInstallment[]): Promise<ContractInstallment[]>;
  getContractInstallments(contractId: string): Promise<ContractInstallment[]>;
  updateContractInstallment(id: string, data: Partial<InsertContractInstallment>): Promise<ContractInstallment | undefined>;
  deleteContractInstallments(contractId: string): Promise<void>;

  // Lead Workflows
  createLeadWorkflow(workflow: InsertLeadWorkflow): Promise<LeadWorkflow>;
  getLeadWorkflowsByWorkspace(workspaceId: string): Promise<LeadWorkflow[]>;
  getLeadWorkflowById(id: string, workspaceId: string): Promise<LeadWorkflow | undefined>;
  updateLeadWorkflow(id: string, workspaceId: string, data: Partial<InsertLeadWorkflow>): Promise<LeadWorkflow | undefined>;
  deleteLeadWorkflow(id: string, workspaceId: string): Promise<void>;

  // Lead Workflow Stages
  createLeadWorkflowStage(stage: InsertLeadWorkflowStage): Promise<LeadWorkflowStage>;
  getLeadWorkflowStages(workflowId: string): Promise<LeadWorkflowStage[]>;
  getLeadWorkflowStageById(id: string): Promise<LeadWorkflowStage | undefined>;
  updateLeadWorkflowStage(id: string, data: Partial<InsertLeadWorkflowStage>): Promise<LeadWorkflowStage | undefined>;
  deleteLeadWorkflowStage(id: string): Promise<void>;
  reorderLeadWorkflowStages(workflowId: string, stageIds: string[]): Promise<void>;

  // Lead Forms
  createLeadForm(form: InsertLeadForm): Promise<LeadForm>;
  getLeadFormsByWorkspace(workspaceId: string): Promise<LeadForm[]>;
  getLeadFormById(id: string, workspaceId: string): Promise<LeadForm | undefined>;
  updateLeadForm(id: string, workspaceId: string, data: Partial<InsertLeadForm>): Promise<LeadForm | undefined>;
  deleteLeadForm(id: string, workspaceId: string): Promise<void>;

  // Lead Form Fields
  createLeadFormField(field: InsertLeadFormField): Promise<LeadFormField>;
  getLeadFormFields(formId: string): Promise<LeadFormField[]>;
  updateLeadFormField(id: string, data: Partial<InsertLeadFormField>): Promise<LeadFormField | undefined>;
  deleteLeadFormField(id: string): Promise<void>;
  reorderLeadFormFields(formId: string, fieldIds: string[]): Promise<void>;

  // Lead Landing Pages
  createLeadLandingPage(page: InsertLeadLandingPage): Promise<LeadLandingPage>;
  getLeadLandingPagesByWorkspace(workspaceId: string): Promise<LeadLandingPage[]>;
  getLeadLandingPageById(id: string, workspaceId: string): Promise<LeadLandingPage | undefined>;
  getLeadLandingPageBySlug(slug: string): Promise<LeadLandingPage | undefined>;
  updateLeadLandingPage(id: string, workspaceId: string, data: Partial<InsertLeadLandingPage>): Promise<LeadLandingPage | undefined>;
  deleteLeadLandingPage(id: string, workspaceId: string): Promise<void>;

  // Leads
  createLead(lead: InsertLead): Promise<Lead>;
  getLeadsByWorkspace(workspaceId: string, filters?: { workflowId?: string; stageId?: string }): Promise<Lead[]>;
  getLeadById(id: string, workspaceId: string): Promise<Lead | undefined>;
  updateLead(id: string, workspaceId: string, data: Partial<InsertLead>): Promise<Lead | undefined>;
  deleteLead(id: string, workspaceId: string): Promise<void>;
  incrementLeadReferralCount(referralCode: string): Promise<void>;
  
  // Lead Cleanup
  getLeadsWithoutContact(workspaceId: string): Promise<Lead[]>;
  getDuplicateLeads(workspaceId: string): Promise<{ email: string | null; phone: string | null; leads: Lead[] }[]>;
  deleteLeadsBatch(workspaceId: string, leadIds: string[]): Promise<number>;

  // Lead Form Submissions
  createLeadFormSubmission(submission: InsertLeadFormSubmission): Promise<LeadFormSubmission>;
  getLeadFormSubmissions(leadId: string): Promise<LeadFormSubmission[]>;

  // Lead Stage History
  createLeadStageHistory(history: InsertLeadStageHistory): Promise<LeadStageHistory>;
  getLeadStageHistory(leadId: string): Promise<LeadStageHistory[]>;

  // Custom Field Definitions
  createCustomFieldDefinition(definition: InsertCustomFieldDefinition): Promise<CustomFieldDefinition>;
  getCustomFieldDefinitionsByWorkspace(workspaceId: string, entityType?: string): Promise<CustomFieldDefinition[]>;
  getCustomFieldDefinitionById(id: string, workspaceId: string): Promise<CustomFieldDefinition | undefined>;
  updateCustomFieldDefinition(id: string, workspaceId: string, data: Partial<InsertCustomFieldDefinition>): Promise<CustomFieldDefinition | undefined>;
  deleteCustomFieldDefinition(id: string, workspaceId: string): Promise<void>;
  reorderCustomFieldDefinitions(workspaceId: string, entityType: string, definitionIds: string[]): Promise<void>;

  // Custom Field Values
  getCustomFieldValues(workspaceId: string, entityType: string, entityId: string): Promise<CustomFieldValue[]>;
  upsertCustomFieldValues(workspaceId: string, entityType: string, entityId: string, values: { definitionId: string; value: any }[]): Promise<void>;
  deleteCustomFieldValues(entityType: string, entityId: string): Promise<void>;

  // Scheduler Types
  createSchedulerType(schedulerType: InsertSchedulerType): Promise<SchedulerType>;
  getSchedulerTypesByWorkspace(workspaceId: string): Promise<SchedulerType[]>;
  getSchedulerTypeById(id: string, workspaceId: string): Promise<SchedulerType | undefined>;
  getSchedulerTypeBySlug(slug: string, workspaceId: string): Promise<SchedulerType | undefined>;
  updateSchedulerType(id: string, workspaceId: string, data: Partial<InsertSchedulerType>): Promise<SchedulerType | undefined>;
  deleteSchedulerType(id: string, workspaceId: string): Promise<void>;

  // Scheduler Availability
  createSchedulerAvailability(availability: InsertSchedulerAvailability): Promise<SchedulerAvailability>;
  getSchedulerAvailability(schedulerTypeId: string): Promise<SchedulerAvailability[]>;
  deleteSchedulerAvailability(schedulerTypeId: string): Promise<void>;
  replaceSchedulerAvailability(schedulerTypeId: string, availability: InsertSchedulerAvailability[]): Promise<SchedulerAvailability[]>;

  // Scheduler Exceptions
  createSchedulerException(exception: InsertSchedulerException): Promise<SchedulerException>;
  getSchedulerExceptions(schedulerTypeId: string): Promise<SchedulerException[]>;
  deleteSchedulerException(id: string): Promise<void>;

  // Scheduler Bookings
  createSchedulerBooking(booking: InsertSchedulerBooking): Promise<SchedulerBooking>;
  getSchedulerBookingsByWorkspace(workspaceId: string): Promise<SchedulerBooking[]>;
  getSchedulerBookingsBySchedulerType(schedulerTypeId: string): Promise<SchedulerBooking[]>;
  getSchedulerBookingById(id: string, workspaceId: string): Promise<SchedulerBooking | undefined>;
  getSchedulerBookingsByDateRange(schedulerTypeId: string, startDate: Date, endDate: Date): Promise<SchedulerBooking[]>;
  updateSchedulerBooking(id: string, workspaceId: string, data: Partial<InsertSchedulerBooking>): Promise<SchedulerBooking | undefined>;
  deleteSchedulerBooking(id: string, workspaceId: string): Promise<void>;

  // Site Projects
  createSiteProject(project: InsertSiteProject): Promise<SiteProject>;
  getSiteProjectsByWorkspace(workspaceId: string): Promise<SiteProject[]>;
  getSiteProjectById(id: string, workspaceId: string): Promise<SiteProject | undefined>;
  getSiteProjectByIdForValidation(id: string): Promise<SiteProject | undefined>;
  getSiteProjectBySlug(workspaceId: string, slug: string): Promise<SiteProject | undefined>;
  updateSiteProject(id: string, workspaceId: string, data: Partial<InsertSiteProject>): Promise<SiteProject | undefined>;
  deleteSiteProject(id: string, workspaceId: string): Promise<void>;

  // Site Pages
  createSitePage(page: InsertSitePage): Promise<SitePage>;
  getSitePagesBySite(siteId: string): Promise<SitePage[]>;
  getSitePageById(id: string): Promise<SitePage | undefined>;
  getSitePageBySlug(siteId: string, slug: string): Promise<SitePage | undefined>;
  updateSitePage(id: string, data: Partial<InsertSitePage>): Promise<SitePage | undefined>;
  deleteSitePage(id: string): Promise<void>;

  // Site Page Versions
  createSitePageVersion(version: InsertSitePageVersion): Promise<SitePageVersion>;
  getSitePageVersions(pageId: string): Promise<SitePageVersion[]>;

  // Blog Categories
  createBlogCategory(category: InsertBlogCategory): Promise<BlogCategory>;
  getBlogCategoriesBySite(siteId: string): Promise<BlogCategory[]>;
  getBlogCategoryById(id: string): Promise<BlogCategory | undefined>;
  updateBlogCategory(id: string, data: Partial<InsertBlogCategory>): Promise<BlogCategory | undefined>;
  deleteBlogCategory(id: string): Promise<void>;

  // Blog Posts
  createBlogPost(post: InsertBlogPost): Promise<BlogPost>;
  getBlogPostsBySite(siteId: string): Promise<BlogPost[]>;
  getBlogPostById(id: string): Promise<BlogPost | undefined>;
  getBlogPostBySlug(siteId: string, slug: string): Promise<BlogPost | undefined>;
  updateBlogPost(id: string, data: Partial<InsertBlogPost>): Promise<BlogPost | undefined>;
  deleteBlogPost(id: string): Promise<void>;
  incrementBlogPostViewCount(id: string): Promise<void>;

  // Blog Post Versions
  createBlogPostVersion(version: InsertBlogPostVersion): Promise<BlogPostVersion>;
  getBlogPostVersions(postId: string): Promise<BlogPostVersion[]>;

  // Site Analytics
  createSiteVisitEvent(event: InsertSiteVisitEvent): Promise<SiteVisitEvent>;
  getSiteVisitStats(siteId: string, startDate: Date, endDate: Date): Promise<SiteVisitDailyStat[]>;

  // Conversations
  getConversations(workspaceId: string): Promise<Conversation[]>;
  getConversation(id: string, workspaceId: string): Promise<Conversation | undefined>;
  createConversation(data: InsertConversation): Promise<Conversation>;
  getOrCreateDirectConversation(workspaceId: string, userId1: string, userId2: string): Promise<Conversation>;

  // Conversation Participants
  getConversationParticipants(conversationId: string): Promise<ConversationParticipant[]>;
  addConversationParticipant(data: InsertConversationParticipant): Promise<ConversationParticipant>;
  updateParticipantLastRead(conversationId: string, userId: string): Promise<void>;

  // Messages
  getMessages(conversationId: string, limit?: number, offset?: number): Promise<Message[]>;
  getMessage(id: string): Promise<Message | undefined>;
  createMessage(data: InsertMessage): Promise<Message>;
  updateMessage(id: string, content: string): Promise<Message | undefined>;
  deleteMessage(id: string): Promise<void>;

  // Comments
  getComments(workspaceId: string, entityType: string, entityId: string): Promise<Comment[]>;
  createComment(data: InsertComment): Promise<Comment>;
  updateComment(id: string, content: string): Promise<Comment | undefined>;
  deleteComment(id: string): Promise<void>;

  // Files
  getFiles(workspaceId: string, options?: { projectId?: string; clientId?: string; taskId?: string; folder?: string }): Promise<File[]>;
  getFile(id: string, workspaceId: string): Promise<File | undefined>;
  createFile(data: InsertFile): Promise<File>;
  deleteFile(id: string, workspaceId: string): Promise<void>;

  // Client Documents
  getClientDocuments(workspaceId: string, clientId: string): Promise<ClientDocument[]>;
  getExpiringDocuments(workspaceId: string, daysAhead: number): Promise<ClientDocument[]>;
  getClientDocument(id: string, workspaceId: string): Promise<ClientDocument | undefined>;
  createClientDocument(data: InsertClientDocument): Promise<ClientDocument>;
  updateClientDocument(id: string, data: Partial<InsertClientDocument>): Promise<ClientDocument | undefined>;
  deleteClientDocument(id: string, workspaceId: string): Promise<void>;

  // Notifications
  getNotifications(workspaceId: string, userId: string): Promise<Notification[]>;
  getUnreadNotificationCount(workspaceId: string, userId: string): Promise<number>;
  createNotification(data: InsertNotification): Promise<Notification>;
  markNotificationRead(id: string): Promise<Notification | undefined>;
  markAllNotificationsRead(workspaceId: string, userId: string): Promise<void>;

  // Host Panel - Users
  updateUserGlobalRole(userId: string, role: GlobalRole): Promise<User | undefined>;
  getAllUsers(): Promise<User[]>;

  // Host Panel - Workspaces
  getAllWorkspaces(): Promise<Workspace[]>;
  getWorkspaceWithSubscription(workspaceId: string): Promise<(Workspace & { subscription?: WorkspaceSubscription }) | undefined>;
  getAllWorkspacesWithSubscriptions(): Promise<(Workspace & { subscription?: WorkspaceSubscription })[]>;

  // Host Panel - Subscriptions
  getWorkspaceSubscription(workspaceId: string): Promise<WorkspaceSubscription | undefined>;
  createWorkspaceSubscription(subscription: InsertWorkspaceSubscription): Promise<WorkspaceSubscription>;
  updateWorkspaceSubscription(workspaceId: string, data: Partial<InsertWorkspaceSubscription>): Promise<WorkspaceSubscription | undefined>;

  // Host Panel - Usage Metrics
  getWorkspaceUsageMetrics(workspaceId: string, startDate: Date, endDate: Date): Promise<WorkspaceUsageMetrics[]>;
  upsertWorkspaceUsageMetrics(data: InsertWorkspaceUsageMetrics): Promise<WorkspaceUsageMetrics>;
  getAggregatedUsageMetrics(): Promise<{
    totalWorkspaces: number;
    totalUsers: number;
    totalClients: number;
    totalProjects: number;
    mrr: number;
    arr: number;
    newWorkspacesThisMonth: number;
    churnedWorkspacesThisMonth: number;
    churnRate: number;
    activeUsersLast30Days: number;
    avgRevenuePerWorkspace: number;
  }>;

  // Host Panel - Access Events
  createWorkspaceAccessEvent(event: InsertWorkspaceAccessEvent): Promise<WorkspaceAccessEvent>;
  getWorkspaceAccessEvents(workspaceId: string, limit?: number): Promise<WorkspaceAccessEvent[]>;
  getRecentAccessEvents(limit?: number): Promise<(WorkspaceAccessEvent & { workspace?: Workspace; user?: User })[]>;

  // Landing Page Leads
  createLandingLead(lead: InsertLandingLead): Promise<LandingLead>;
  getLandingLeads(): Promise<LandingLead[]>;
  updateLandingLead(id: string, data: Partial<InsertLandingLead>): Promise<LandingLead | undefined>;

  // Email System
  createEmailLog(data: InsertEmailLog): Promise<EmailLog>;
  getEmailLogById(id: string): Promise<EmailLog | undefined>;
  updateEmailLog(id: string, data: Partial<InsertEmailLog>): Promise<EmailLog | undefined>;
  getEmailLogsByWorkspace(workspaceId: string): Promise<EmailLog[]>;
  getEmailTemplatesByWorkspace(workspaceId: string): Promise<EmailTemplate[]>;
  getEmailTemplateById(id: string): Promise<EmailTemplate | undefined>;
  createEmailTemplate(data: InsertEmailTemplate): Promise<EmailTemplate>;
  updateEmailTemplate(id: string, workspaceId: string, data: Partial<InsertEmailTemplate>): Promise<EmailTemplate | undefined>;
  deleteEmailTemplate(id: string, workspaceId: string): Promise<void>;

  // SMS System
  getSmsTemplatesByWorkspace(workspaceId: string): Promise<SmsTemplate[]>;
  getSmsTemplateById(id: string): Promise<SmsTemplate | undefined>;
  createSmsTemplate(data: InsertSmsTemplate): Promise<SmsTemplate>;
  updateSmsTemplate(id: string, workspaceId: string, data: Partial<InsertSmsTemplate>): Promise<SmsTemplate | undefined>;
  deleteSmsTemplate(id: string, workspaceId: string): Promise<void>;
  getSmsLogsByWorkspace(workspaceId: string): Promise<SmsLog[]>;

  // WhatsApp System
  getWhatsappTemplatesByWorkspace(workspaceId: string): Promise<WhatsappTemplate[]>;
  getWhatsappTemplateById(id: string): Promise<WhatsappTemplate | undefined>;
  createWhatsappTemplate(data: InsertWhatsappTemplate): Promise<WhatsappTemplate>;
  updateWhatsappTemplate(id: string, workspaceId: string, data: Partial<InsertWhatsappTemplate>): Promise<WhatsappTemplate | undefined>;
  deleteWhatsappTemplate(id: string, workspaceId: string): Promise<void>;
  getWhatsappLogsByWorkspace(workspaceId: string): Promise<WhatsappLog[]>;

  // Workspace Google Credentials
  getWorkspaceGoogleCredentials(workspaceId: string): Promise<WorkspaceGoogleCredentials | undefined>;
  saveWorkspaceGoogleCredentials(data: InsertWorkspaceGoogleCredentials): Promise<WorkspaceGoogleCredentials>;
  deleteWorkspaceGoogleCredentials(workspaceId: string): Promise<void>;

  // Workspace Google Calendar Credentials
  getWorkspaceGoogleCalendarCredentials(workspaceId: string): Promise<WorkspaceGoogleCalendarCredentials | undefined>;
  saveWorkspaceGoogleCalendarCredentials(data: InsertWorkspaceGoogleCalendarCredentials): Promise<WorkspaceGoogleCalendarCredentials>;
  deleteWorkspaceGoogleCalendarCredentials(workspaceId: string): Promise<void>;

  // Project Responsibles
  addProjectResponsible(projectId: string, memberId: string): Promise<ProjectResponsible>;
  getProjectResponsibles(projectId: string): Promise<(ProjectResponsible & { member: WorkspaceMember & { user: User } })[]>;
  removeProjectResponsible(projectId: string, memberId: string): Promise<void>;

  // Project Followers
  addProjectFollower(projectId: string, memberId: string): Promise<ProjectFollower>;
  getProjectFollowers(projectId: string): Promise<(ProjectFollower & { member: WorkspaceMember & { user: User } })[]>;
  removeProjectFollower(projectId: string, memberId: string): Promise<void>;

  // Project Communications
  createProjectCommunication(data: InsertProjectCommunication): Promise<ProjectCommunication>;
  getProjectCommunications(projectId: string): Promise<ProjectCommunication[]>;
  getProjectCommunicationById(id: string, workspaceId: string): Promise<ProjectCommunication | undefined>;
  updateProjectCommunication(id: string, workspaceId: string, data: Partial<InsertProjectCommunication>): Promise<ProjectCommunication | undefined>;
  deleteProjectCommunication(id: string, workspaceId: string): Promise<void>;
}

// Implementation
class Storage implements IStorage {
  // Users
  async createUser(user: InsertUser): Promise<User> {
    const [newUser] = await db.insert(schema.users).values(user).returning();
    return newUser;
  }

  async getUserById(id: string): Promise<User | undefined> {
    return await db.query.users.findFirst({ where: eq(schema.users.id, id) });
  }

  async getUserByEmail(email: string): Promise<User | undefined> {
    return await db.query.users.findFirst({ where: eq(schema.users.email, email) });
  }

  // Workspaces
  async createWorkspace(workspace: InsertWorkspace): Promise<Workspace> {
    const [newWorkspace] = await db.insert(schema.workspaces).values(workspace).returning();
    return newWorkspace;
  }

  async getWorkspaceBySlug(slug: string): Promise<Workspace | undefined> {
    return await db.query.workspaces.findFirst({ where: eq(schema.workspaces.slug, slug) });
  }

  async getWorkspaceById(id: string): Promise<Workspace | undefined> {
    return await db.query.workspaces.findFirst({ where: eq(schema.workspaces.id, id) });
  }

  async getUserWorkspaces(userId: string): Promise<Workspace[]> {
    const memberships = await db.query.workspaceMembers.findMany({
      where: eq(schema.workspaceMembers.userId, userId),
    });
    
    const workspaceIds = memberships.map(m => m.workspaceId);
    if (workspaceIds.length === 0) return [];
    
    const workspaces = await Promise.all(
      workspaceIds.map(id => this.getWorkspaceById(id))
    );
    
    return workspaces.filter((w): w is Workspace => w !== undefined);
  }

  async updateWorkspace(workspaceId: string, data: Partial<InsertWorkspace>): Promise<Workspace | undefined> {
    const [updated] = await db
      .update(schema.workspaces)
      .set(data)
      .where(eq(schema.workspaces.id, workspaceId))
      .returning();
    return updated;
  }

  // Workspace Members
  async addWorkspaceMember(member: InsertWorkspaceMember): Promise<WorkspaceMember> {
    const [newMember] = await db.insert(schema.workspaceMembers).values(member).returning();
    return newMember;
  }

  async getWorkspaceMembers(workspaceId: string): Promise<WorkspaceMember[]> {
    return await db.query.workspaceMembers.findMany({
      where: eq(schema.workspaceMembers.workspaceId, workspaceId),
    });
  }

  async getUserWorkspaceMembership(userId: string, workspaceId: string): Promise<WorkspaceMember | undefined> {
    return await db.query.workspaceMembers.findFirst({
      where: and(
        eq(schema.workspaceMembers.userId, userId),
        eq(schema.workspaceMembers.workspaceId, workspaceId)
      ),
    });
  }

  async getWorkspaceMembersWithUsers(workspaceId: string): Promise<(WorkspaceMember & { user: User })[]> {
    const members = await db.query.workspaceMembers.findMany({
      where: eq(schema.workspaceMembers.workspaceId, workspaceId),
    });
    
    const result: (WorkspaceMember & { user: User })[] = [];
    for (const member of members) {
      const user = await this.getUserById(member.userId);
      if (user) {
        result.push({ ...member, user });
      }
    }
    return result;
  }

  async updateWorkspaceMember(memberId: string, workspaceId: string, data: { memberRole?: MemberRole }): Promise<WorkspaceMember | undefined> {
    const [updated] = await db
      .update(schema.workspaceMembers)
      .set(data as any)
      .where(and(eq(schema.workspaceMembers.id, memberId), eq(schema.workspaceMembers.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async removeWorkspaceMember(memberId: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.workspaceMembers)
      .where(and(eq(schema.workspaceMembers.id, memberId), eq(schema.workspaceMembers.workspaceId, workspaceId)));
  }

  // User Profile
  async updateUser(id: string, data: Partial<InsertUser>): Promise<User | undefined> {
    const [updated] = await db
      .update(schema.users)
      .set(data)
      .where(eq(schema.users.id, id))
      .returning();
    return updated;
  }

  // Member Permissions
  async getMemberPermissions(memberId: string, workspaceId: string): Promise<MemberPermission[]> {
    return await db.query.memberPermissions.findMany({
      where: and(
        eq(schema.memberPermissions.memberId, memberId),
        eq(schema.memberPermissions.workspaceId, workspaceId)
      ),
    });
  }

  async setMemberPermission(data: InsertMemberPermission): Promise<MemberPermission> {
    const existing = await db.query.memberPermissions.findFirst({
      where: and(
        eq(schema.memberPermissions.memberId, data.memberId),
        eq(schema.memberPermissions.module, data.module),
        eq(schema.memberPermissions.action, data.action),
        eq(schema.memberPermissions.workspaceId, data.workspaceId)
      ),
    });

    if (existing) {
      const [updated] = await db
        .update(schema.memberPermissions)
        .set({ granted: data.granted })
        .where(eq(schema.memberPermissions.id, existing.id))
        .returning();
      return updated;
    }

    const [permission] = await db.insert(schema.memberPermissions).values(data).returning();
    return permission;
  }

  async removeMemberPermission(memberId: string, module: PermissionModule, action: PermissionAction, workspaceId: string): Promise<void> {
    await db
      .delete(schema.memberPermissions)
      .where(and(
        eq(schema.memberPermissions.memberId, memberId),
        eq(schema.memberPermissions.module, module as any),
        eq(schema.memberPermissions.action, action as any),
        eq(schema.memberPermissions.workspaceId, workspaceId)
      ));
  }

  async clearMemberPermissions(memberId: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.memberPermissions)
      .where(and(
        eq(schema.memberPermissions.memberId, memberId),
        eq(schema.memberPermissions.workspaceId, workspaceId)
      ));
  }

  async checkPermission(memberId: string, workspaceId: string, module: PermissionModule, action: PermissionAction): Promise<boolean> {
    const member = await db.query.workspaceMembers.findFirst({
      where: eq(schema.workspaceMembers.id, memberId),
    });
    if (!member) return false;

    // OWNER always has full access
    if (member.role === 'OWNER') return true;

    // Get default permissions for the role
    const memberRole = member.memberRole as MemberRole;
    const defaultPerms = schema.DEFAULT_ROLE_PERMISSIONS[memberRole];
    const hasDefaultPermission = defaultPerms?.[module]?.includes(action) ?? false;

    // Check for extra permissions granted
    const extraGrant = await db.query.memberPermissions.findFirst({
      where: and(
        eq(schema.memberPermissions.memberId, memberId),
        eq(schema.memberPermissions.module, module as any),
        eq(schema.memberPermissions.action, action as any),
        eq(schema.memberPermissions.workspaceId, workspaceId),
        eq(schema.memberPermissions.granted, true)
      ),
    });

    // Check if permission was explicitly revoked
    const explicitRevoke = await db.query.memberPermissions.findFirst({
      where: and(
        eq(schema.memberPermissions.memberId, memberId),
        eq(schema.memberPermissions.module, module as any),
        eq(schema.memberPermissions.action, action as any),
        eq(schema.memberPermissions.workspaceId, workspaceId),
        eq(schema.memberPermissions.granted, false)
      ),
    });

    if (explicitRevoke) return false;
    if (extraGrant) return true;
    return hasDefaultPermission;
  }

  // Workspace Invites
  async createWorkspaceInvite(invite: InsertWorkspaceInvite): Promise<WorkspaceInvite> {
    const [newInvite] = await db.insert(schema.workspaceInvites).values(invite).returning();
    return newInvite;
  }

  async getWorkspaceInvites(workspaceId: string): Promise<WorkspaceInvite[]> {
    return await db.query.workspaceInvites.findMany({
      where: eq(schema.workspaceInvites.workspaceId, workspaceId),
      orderBy: [desc(schema.workspaceInvites.createdAt)],
    });
  }

  async getInviteByToken(token: string): Promise<WorkspaceInvite | undefined> {
    return await db.query.workspaceInvites.findFirst({
      where: eq(schema.workspaceInvites.token, token),
    });
  }

  async updateInviteStatus(id: string, status: 'ACCEPTED' | 'EXPIRED' | 'CANCELLED'): Promise<WorkspaceInvite | undefined> {
    const [updated] = await db
      .update(schema.workspaceInvites)
      .set({ status: status as any, acceptedAt: status === 'ACCEPTED' ? new Date() : undefined })
      .where(eq(schema.workspaceInvites.id, id))
      .returning();
    return updated;
  }

  async deleteWorkspaceInvite(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.workspaceInvites)
      .where(and(eq(schema.workspaceInvites.id, id), eq(schema.workspaceInvites.workspaceId, workspaceId)));
  }

  // Clients
  async createClient(client: InsertClient): Promise<Client> {
    const [newClient] = await db.insert(schema.clients).values(client).returning();
    return newClient;
  }

  async getClientsByWorkspace(workspaceId: string): Promise<Client[]> {
    return await db.query.clients.findMany({
      where: eq(schema.clients.workspaceId, workspaceId),
      orderBy: [desc(schema.clients.createdAt)],
    });
  }

  async getClientsByWorkspacePaginated(
    workspaceId: string, 
    options: { 
      page?: number; 
      pageSize?: number; 
      search?: string;
      clientType?: string;
      sortBy?: string;
    } = {}
  ): Promise<{ items: Client[]; total: number; page: number; pageSize: number }> {
    const { page = 1, pageSize = 20, search, clientType, sortBy = 'newest' } = options;
    const offset = (page - 1) * pageSize;
    
    let conditions: any[] = [eq(schema.clients.workspaceId, workspaceId)];
    
    if (clientType) {
      conditions.push(eq(schema.clients.clientType, clientType as any));
    }
    if (search) {
      conditions.push(
        or(
          ilike(schema.clients.name, `%${search}%`),
          ilike(schema.clients.email, `%${search}%`),
          ilike(schema.clients.phone, `%${search}%`),
          ilike(schema.clients.companyName, `%${search}%`),
          ilike(schema.clients.cnpj, `%${search}%`),
          ilike(schema.clients.cpf, `%${search}%`)
        )
      );
    }
    
    const whereClause = conditions.length > 1 ? and(...conditions) : conditions[0];
    
    const countResult = await db.select({ count: sql<number>`count(*)` })
      .from(schema.clients)
      .where(whereClause);
    const total = Number(countResult[0]?.count || 0);
    
    let orderByClause: any[];
    switch (sortBy) {
      case 'oldest':
        orderByClause = [asc(schema.clients.createdAt)];
        break;
      case 'name_asc':
        orderByClause = [asc(schema.clients.name)];
        break;
      case 'name_desc':
        orderByClause = [desc(schema.clients.name)];
        break;
      case 'updated':
        orderByClause = [desc(schema.clients.updatedAt)];
        break;
      case 'newest':
      default:
        orderByClause = [desc(schema.clients.createdAt)];
        break;
    }
    
    const items = await db.query.clients.findMany({
      where: whereClause,
      orderBy: orderByClause,
      limit: pageSize,
      offset,
    });
    
    return { items, total, page, pageSize };
  }

  async getClientById(id: string, workspaceId: string): Promise<Client | undefined> {
    return await db.query.clients.findFirst({
      where: and(eq(schema.clients.id, id), eq(schema.clients.workspaceId, workspaceId)),
    });
  }

  async updateClient(id: string, workspaceId: string, data: Partial<InsertClient>): Promise<Client | undefined> {
    const [updated] = await db
      .update(schema.clients)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.clients.id, id), eq(schema.clients.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteClient(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.clients)
      .where(and(eq(schema.clients.id, id), eq(schema.clients.workspaceId, workspaceId)));
  }

  // Client Contacts
  async createClientContact(contact: InsertClientContact): Promise<ClientContact> {
    const [newContact] = await db.insert(schema.clientContacts).values(contact).returning();
    return newContact;
  }

  async getClientContacts(clientId: string): Promise<ClientContact[]> {
    return await db.query.clientContacts.findMany({
      where: eq(schema.clientContacts.clientId, clientId),
      orderBy: [desc(schema.clientContacts.isPrimary), desc(schema.clientContacts.createdAt)],
    });
  }

  async updateClientContact(id: string, data: Partial<InsertClientContact>): Promise<ClientContact | undefined> {
    const [updated] = await db
      .update(schema.clientContacts)
      .set(data)
      .where(eq(schema.clientContacts.id, id))
      .returning();
    return updated;
  }

  async deleteClientContact(id: string): Promise<void> {
    await db.delete(schema.clientContacts).where(eq(schema.clientContacts.id, id));
  }

  async setPrimaryContact(clientId: string, contactId: string): Promise<void> {
    await db
      .update(schema.clientContacts)
      .set({ isPrimary: false })
      .where(eq(schema.clientContacts.clientId, clientId));
    await db
      .update(schema.clientContacts)
      .set({ isPrimary: true })
      .where(eq(schema.clientContacts.id, contactId));
  }

  // Client Messages
  async createClientMessage(message: InsertClientMessage): Promise<ClientMessage> {
    const [newMessage] = await db.insert(schema.clientMessages).values(message).returning();
    return newMessage;
  }

  async getClientMessages(clientId: string, workspaceId: string): Promise<ClientMessage[]> {
    return await db
      .select()
      .from(schema.clientMessages)
      .where(and(eq(schema.clientMessages.clientId, clientId), eq(schema.clientMessages.workspaceId, workspaceId)))
      .orderBy(desc(schema.clientMessages.createdAt));
  }

  async updateClientMessage(id: string, workspaceId: string, data: Partial<InsertClientMessage>): Promise<ClientMessage | undefined> {
    const [updated] = await db
      .update(schema.clientMessages)
      .set(data)
      .where(and(eq(schema.clientMessages.id, id), eq(schema.clientMessages.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteClientMessage(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.clientMessages)
      .where(and(eq(schema.clientMessages.id, id), eq(schema.clientMessages.workspaceId, workspaceId)));
  }

  // Projects
  async createProject(project: InsertProject): Promise<Project> {
    const [newProject] = await db.insert(schema.projects).values(project).returning();
    return newProject;
  }

  async getProjectsByWorkspace(workspaceId: string): Promise<Project[]> {
    return await db.query.projects.findMany({
      where: eq(schema.projects.workspaceId, workspaceId),
      orderBy: [asc(schema.projects.position), desc(schema.projects.createdAt)],
    });
  }

  async getProjectById(id: string, workspaceId: string): Promise<Project | undefined> {
    return await db.query.projects.findFirst({
      where: and(eq(schema.projects.id, id), eq(schema.projects.workspaceId, workspaceId)),
    });
  }

  async updateProject(id: string, workspaceId: string, data: Partial<InsertProject>): Promise<Project | undefined> {
    const [updated] = await db
      .update(schema.projects)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.projects.id, id), eq(schema.projects.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteProject(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.projects)
      .where(and(eq(schema.projects.id, id), eq(schema.projects.workspaceId, workspaceId)));
  }

  async reorderProjects(workspaceId: string, projectIds: string[], status: string): Promise<void> {
    for (let i = 0; i < projectIds.length; i++) {
      await db
        .update(schema.projects)
        .set({ position: i, status: status as any, updatedAt: new Date() })
        .where(and(eq(schema.projects.id, projectIds[i]), eq(schema.projects.workspaceId, workspaceId)));
    }
  }

  // Tasks
  async createTask(task: InsertTask): Promise<Task> {
    const [newTask] = await db.insert(schema.tasks).values(task).returning();
    return newTask;
  }

  async getTasksByWorkspace(workspaceId: string): Promise<any[]> {
    const tasksData = await db.query.tasks.findMany({
      where: eq(schema.tasks.workspaceId, workspaceId),
      orderBy: [schema.tasks.position, desc(schema.tasks.createdAt)],
    });
    
    const tasksWithAssignees = await Promise.all(
      tasksData.map(async (task) => {
        let assignee = null;
        if (task.assigneeId) {
          assignee = await db.query.users.findFirst({
            where: eq(schema.users.id, task.assigneeId),
            columns: { id: true, name: true, email: true },
          });
        }
        return { ...task, assignee };
      })
    );
    
    return tasksWithAssignees;
  }

  async getTasksByProject(projectId: string, workspaceId: string): Promise<any[]> {
    const tasksData = await db.query.tasks.findMany({
      where: and(eq(schema.tasks.projectId, projectId), eq(schema.tasks.workspaceId, workspaceId)),
      orderBy: [desc(schema.tasks.createdAt)],
    });
    
    const tasksWithAssignees = await Promise.all(
      tasksData.map(async (task) => {
        let assignee = null;
        if (task.assigneeId) {
          assignee = await db.query.users.findFirst({
            where: eq(schema.users.id, task.assigneeId),
            columns: { id: true, name: true, email: true },
          });
        }
        return { ...task, assignee };
      })
    );
    
    return tasksWithAssignees;
  }

  async getTaskById(id: string, workspaceId: string): Promise<Task | undefined> {
    return await db.query.tasks.findFirst({
      where: and(eq(schema.tasks.id, id), eq(schema.tasks.workspaceId, workspaceId)),
    });
  }

  async updateTask(id: string, workspaceId: string, data: Partial<InsertTask>): Promise<Task | undefined> {
    const [updated] = await db
      .update(schema.tasks)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.tasks.id, id), eq(schema.tasks.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteTask(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.tasks)
      .where(and(eq(schema.tasks.id, id), eq(schema.tasks.workspaceId, workspaceId)));
  }

  async toggleTaskComplete(id: string, workspaceId: string): Promise<Task | undefined> {
    const task = await this.getTaskById(id, workspaceId);
    if (!task) return undefined;
    
    return await this.updateTask(id, workspaceId, { isCompleted: !task.isCompleted });
  }

  async reorderTasks(workspaceId: string, taskIds: string[], isCompleted: boolean): Promise<void> {
    for (let i = 0; i < taskIds.length; i++) {
      await db
        .update(schema.tasks)
        .set({ position: i, isCompleted, updatedAt: new Date() })
        .where(and(eq(schema.tasks.id, taskIds[i]), eq(schema.tasks.workspaceId, workspaceId)));
    }
  }

  // Task Comments
  async createTaskComment(comment: InsertTaskComment): Promise<TaskComment> {
    const [newComment] = await db.insert(schema.taskComments).values(comment).returning();
    return newComment;
  }

  async getTaskComments(taskId: string): Promise<TaskComment[]> {
    return await db.query.taskComments.findMany({
      where: eq(schema.taskComments.taskId, taskId),
      orderBy: [schema.taskComments.createdAt],
    });
  }

  async deleteTaskComment(id: string): Promise<void> {
    await db.delete(schema.taskComments).where(eq(schema.taskComments.id, id));
  }

  // Invoices
  async createInvoice(invoice: InsertInvoice): Promise<Invoice> {
    const [newInvoice] = await db.insert(schema.invoices).values(invoice).returning();
    return newInvoice;
  }

  async getInvoicesByWorkspace(workspaceId: string): Promise<Invoice[]> {
    return await db.query.invoices.findMany({
      where: eq(schema.invoices.workspaceId, workspaceId),
      orderBy: [desc(schema.invoices.createdAt)],
    });
  }

  async getInvoiceById(id: string, workspaceId: string): Promise<Invoice | undefined> {
    return await db.query.invoices.findFirst({
      where: and(eq(schema.invoices.id, id), eq(schema.invoices.workspaceId, workspaceId)),
    });
  }

  async updateInvoice(id: string, workspaceId: string, data: Partial<InsertInvoice>): Promise<Invoice | undefined> {
    const [updated] = await db
      .update(schema.invoices)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.invoices.id, id), eq(schema.invoices.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteInvoice(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.invoices)
      .where(and(eq(schema.invoices.id, id), eq(schema.invoices.workspaceId, workspaceId)));
  }

  // Invoice Items
  async createInvoiceItem(item: InsertInvoiceItem): Promise<InvoiceItem> {
    const [newItem] = await db.insert(schema.invoiceItems).values(item).returning();
    return newItem;
  }

  async getInvoiceItems(invoiceId: string): Promise<InvoiceItem[]> {
    return await db.query.invoiceItems.findMany({
      where: eq(schema.invoiceItems.invoiceId, invoiceId),
    });
  }

  async deleteInvoiceItems(invoiceId: string): Promise<void> {
    await db.delete(schema.invoiceItems).where(eq(schema.invoiceItems.invoiceId, invoiceId));
  }

  // Proposals
  async createProposal(proposal: InsertProposal): Promise<Proposal> {
    const [newProposal] = await db.insert(schema.proposals).values(proposal).returning();
    return newProposal;
  }

  async getProposalsByWorkspace(workspaceId: string): Promise<Proposal[]> {
    return await db.query.proposals.findMany({
      where: eq(schema.proposals.workspaceId, workspaceId),
      orderBy: [desc(schema.proposals.createdAt)],
    });
  }

  async getProposalById(id: string, workspaceId: string): Promise<Proposal | undefined> {
    return await db.query.proposals.findFirst({
      where: and(eq(schema.proposals.id, id), eq(schema.proposals.workspaceId, workspaceId)),
    });
  }

  async getProposalByToken(token: string): Promise<Proposal | undefined> {
    return await db.query.proposals.findFirst({
      where: eq(schema.proposals.publicToken, token),
    });
  }

  async updateProposal(id: string, workspaceId: string, data: Partial<InsertProposal>): Promise<Proposal | undefined> {
    const [updated] = await db
      .update(schema.proposals)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.proposals.id, id), eq(schema.proposals.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteProposal(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.proposals)
      .where(and(eq(schema.proposals.id, id), eq(schema.proposals.workspaceId, workspaceId)));
  }

  // Proposal Items
  async createProposalItem(item: InsertProposalItem): Promise<ProposalItem> {
    const [newItem] = await db.insert(schema.proposalItems).values(item).returning();
    return newItem;
  }

  async getProposalItems(proposalId: string): Promise<ProposalItem[]> {
    return await db.query.proposalItems.findMany({
      where: eq(schema.proposalItems.proposalId, proposalId),
    });
  }

  async deleteProposalItems(proposalId: string): Promise<void> {
    await db.delete(schema.proposalItems).where(eq(schema.proposalItems.proposalId, proposalId));
  }

  // Time Entries
  async createTimeEntry(entry: InsertTimeEntry): Promise<TimeEntry> {
    const [newEntry] = await db.insert(schema.timeEntries).values(entry).returning();
    return newEntry;
  }

  async getTimeEntriesByWorkspace(workspaceId: string): Promise<TimeEntry[]> {
    return await db.query.timeEntries.findMany({
      where: eq(schema.timeEntries.workspaceId, workspaceId),
      orderBy: [desc(schema.timeEntries.date)],
    });
  }

  async getTimeEntriesByProject(projectId: string, workspaceId: string): Promise<TimeEntry[]> {
    return await db.query.timeEntries.findMany({
      where: and(eq(schema.timeEntries.projectId, projectId), eq(schema.timeEntries.workspaceId, workspaceId)),
      orderBy: [desc(schema.timeEntries.date)],
    });
  }

  async updateTimeEntry(id: string, workspaceId: string, data: Partial<InsertTimeEntry>): Promise<TimeEntry | undefined> {
    const [updated] = await db
      .update(schema.timeEntries)
      .set(data)
      .where(and(eq(schema.timeEntries.id, id), eq(schema.timeEntries.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteTimeEntry(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.timeEntries)
      .where(and(eq(schema.timeEntries.id, id), eq(schema.timeEntries.workspaceId, workspaceId)));
  }

  // Financial Categories
  async createFinancialCategory(category: InsertFinancialCategory): Promise<FinancialCategory> {
    const [newCategory] = await db.insert(schema.financialCategories).values(category).returning();
    return newCategory;
  }

  async getFinancialCategoriesByWorkspace(workspaceId: string): Promise<FinancialCategory[]> {
    return await db.query.financialCategories.findMany({
      where: eq(schema.financialCategories.workspaceId, workspaceId),
      orderBy: [schema.financialCategories.name],
    });
  }

  async getFinancialCategoryById(id: string, workspaceId: string): Promise<FinancialCategory | undefined> {
    return await db.query.financialCategories.findFirst({
      where: and(eq(schema.financialCategories.id, id), eq(schema.financialCategories.workspaceId, workspaceId)),
    });
  }

  async updateFinancialCategory(id: string, workspaceId: string, data: Partial<InsertFinancialCategory>): Promise<FinancialCategory | undefined> {
    const [updated] = await db
      .update(schema.financialCategories)
      .set(data)
      .where(and(eq(schema.financialCategories.id, id), eq(schema.financialCategories.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteFinancialCategory(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.financialCategories)
      .where(and(eq(schema.financialCategories.id, id), eq(schema.financialCategories.workspaceId, workspaceId)));
  }

  // Transactions
  async createTransaction(transaction: InsertTransaction): Promise<Transaction> {
    const [newTransaction] = await db.insert(schema.transactions).values(transaction).returning();
    return newTransaction;
  }

  async createTransactionsBatch(transactions: InsertTransaction[]): Promise<Transaction[]> {
    if (transactions.length === 0) return [];
    const newTransactions = await db.insert(schema.transactions).values(transactions).returning();
    return newTransactions;
  }

  async getTransactionsByWorkspace(workspaceId: string): Promise<Transaction[]> {
    return await db.query.transactions.findMany({
      where: eq(schema.transactions.workspaceId, workspaceId),
      orderBy: [desc(schema.transactions.dueDate)],
    });
  }

  async getTransactionById(id: string, workspaceId: string): Promise<Transaction | undefined> {
    return await db.query.transactions.findFirst({
      where: and(eq(schema.transactions.id, id), eq(schema.transactions.workspaceId, workspaceId)),
    });
  }

  async updateTransaction(id: string, workspaceId: string, data: Partial<InsertTransaction>): Promise<Transaction | undefined> {
    const [updated] = await db
      .update(schema.transactions)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.transactions.id, id), eq(schema.transactions.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteTransaction(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.transactions)
      .where(and(eq(schema.transactions.id, id), eq(schema.transactions.workspaceId, workspaceId)));
  }

  async getTransactionsByDateRange(workspaceId: string, startDate: Date, endDate: Date): Promise<Transaction[]> {
    return await db.query.transactions.findMany({
      where: and(
        eq(schema.transactions.workspaceId, workspaceId),
        gte(schema.transactions.dueDate, startDate),
        lte(schema.transactions.dueDate, endDate)
      ),
      orderBy: [schema.transactions.dueDate],
    });
  }

  // Transaction Attachments
  async createTransactionAttachment(attachment: InsertTransactionAttachment): Promise<TransactionAttachment> {
    const [newAttachment] = await db.insert(schema.transactionAttachments).values(attachment).returning();
    return newAttachment;
  }

  async getTransactionAttachments(transactionId: string): Promise<TransactionAttachment[]> {
    return await db.query.transactionAttachments.findMany({
      where: eq(schema.transactionAttachments.transactionId, transactionId),
    });
  }

  async deleteTransactionAttachment(id: string): Promise<void> {
    await db.delete(schema.transactionAttachments).where(eq(schema.transactionAttachments.id, id));
  }

  // Budgets
  async createBudget(budget: InsertBudget): Promise<Budget> {
    const [newBudget] = await db.insert(schema.budgets).values(budget).returning();
    return newBudget;
  }

  async getBudgetsByWorkspace(workspaceId: string, year?: number): Promise<Budget[]> {
    if (year) {
      return await db.query.budgets.findMany({
        where: and(eq(schema.budgets.workspaceId, workspaceId), eq(schema.budgets.year, year)),
      });
    }
    return await db.query.budgets.findMany({
      where: eq(schema.budgets.workspaceId, workspaceId),
    });
  }

  async updateBudget(id: string, data: Partial<InsertBudget>): Promise<Budget | undefined> {
    const [updated] = await db
      .update(schema.budgets)
      .set(data)
      .where(eq(schema.budgets.id, id))
      .returning();
    return updated;
  }

  async deleteBudget(id: string): Promise<void> {
    await db.delete(schema.budgets).where(eq(schema.budgets.id, id));
  }

  // Products
  async createProduct(product: InsertProduct): Promise<Product> {
    const [newProduct] = await db.insert(schema.products).values(product).returning();
    return newProduct;
  }

  async getProductsByWorkspace(workspaceId: string): Promise<Product[]> {
    return await db.query.products.findMany({
      where: eq(schema.products.workspaceId, workspaceId),
      orderBy: [schema.products.name],
    });
  }

  async getProductById(id: string, workspaceId: string): Promise<Product | undefined> {
    return await db.query.products.findFirst({
      where: and(eq(schema.products.id, id), eq(schema.products.workspaceId, workspaceId)),
    });
  }

  async updateProduct(id: string, workspaceId: string, data: Partial<InsertProduct>): Promise<Product | undefined> {
    const [updated] = await db
      .update(schema.products)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.products.id, id), eq(schema.products.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteProduct(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.products)
      .where(and(eq(schema.products.id, id), eq(schema.products.workspaceId, workspaceId)));
  }

  async searchProducts(workspaceId: string, searchTerms: string[]): Promise<Product[]> {
    if (searchTerms.length === 0) return [];
    
    const conditions = searchTerms.map(term => 
      or(
        ilike(schema.products.name, `%${term}%`),
        ilike(schema.products.description, `%${term}%`),
        ilike(schema.products.category, `%${term}%`)
      )
    );
    
    return await db.query.products.findMany({
      where: and(
        eq(schema.products.workspaceId, workspaceId),
        eq(schema.products.isActive, true),
        or(...conditions)
      ),
      orderBy: [schema.products.name],
    });
  }

  async searchClientsFlexible(workspaceId: string, searchTerms: string[]): Promise<Client[]> {
    if (searchTerms.length === 0) return [];
    
    const conditions = searchTerms.map(term => 
      or(
        ilike(schema.clients.name, `%${term}%`),
        ilike(schema.clients.companyName, `%${term}%`),
        ilike(schema.clients.tradeName, `%${term}%`),
        ilike(schema.clients.email, `%${term}%`)
      )
    );
    
    return await db.query.clients.findMany({
      where: and(
        eq(schema.clients.workspaceId, workspaceId),
        or(...conditions)
      ),
      orderBy: [desc(schema.clients.createdAt)],
      limit: 10,
    });
  }

  // Contract Templates
  async createContractTemplate(template: InsertContractTemplate): Promise<ContractTemplate> {
    const [newTemplate] = await db.insert(schema.contractTemplates).values(template).returning();
    return newTemplate;
  }

  async getContractTemplatesByWorkspace(workspaceId: string): Promise<ContractTemplate[]> {
    return await db.query.contractTemplates.findMany({
      where: eq(schema.contractTemplates.workspaceId, workspaceId),
      orderBy: [schema.contractTemplates.name],
    });
  }

  async getContractTemplateById(id: string, workspaceId: string): Promise<ContractTemplate | undefined> {
    return await db.query.contractTemplates.findFirst({
      where: and(eq(schema.contractTemplates.id, id), eq(schema.contractTemplates.workspaceId, workspaceId)),
    });
  }

  async updateContractTemplate(id: string, workspaceId: string, data: Partial<InsertContractTemplate>): Promise<ContractTemplate | undefined> {
    const [updated] = await db
      .update(schema.contractTemplates)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.contractTemplates.id, id), eq(schema.contractTemplates.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteContractTemplate(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.contractTemplates)
      .where(and(eq(schema.contractTemplates.id, id), eq(schema.contractTemplates.workspaceId, workspaceId)));
  }

  // Contracts
  async createContract(contract: InsertContract): Promise<Contract> {
    const [newContract] = await db.insert(schema.contracts).values(contract).returning();
    return newContract;
  }

  async getContractsByWorkspace(workspaceId: string): Promise<Contract[]> {
    return await db.query.contracts.findMany({
      where: eq(schema.contracts.workspaceId, workspaceId),
      orderBy: [desc(schema.contracts.createdAt)],
    });
  }

  async getContractById(id: string, workspaceId: string): Promise<Contract | undefined> {
    return await db.query.contracts.findFirst({
      where: and(eq(schema.contracts.id, id), eq(schema.contracts.workspaceId, workspaceId)),
    });
  }

  async getContractByToken(token: string): Promise<Contract | undefined> {
    return await db.query.contracts.findFirst({
      where: eq(schema.contracts.publicToken, token),
    });
  }

  async updateContract(id: string, workspaceId: string, data: Partial<InsertContract>): Promise<Contract | undefined> {
    const [updated] = await db
      .update(schema.contracts)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.contracts.id, id), eq(schema.contracts.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteContract(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.contracts)
      .where(and(eq(schema.contracts.id, id), eq(schema.contracts.workspaceId, workspaceId)));
  }

  // Contract Items
  async createContractItem(item: InsertContractItem): Promise<ContractItem> {
    const [newItem] = await db.insert(schema.contractItems).values(item).returning();
    return newItem;
  }

  async createContractItemsBatch(items: InsertContractItem[]): Promise<ContractItem[]> {
    if (items.length === 0) return [];
    const newItems = await db.insert(schema.contractItems).values(items).returning();
    return newItems;
  }

  async getContractItems(contractId: string): Promise<ContractItem[]> {
    return await db.query.contractItems.findMany({
      where: eq(schema.contractItems.contractId, contractId),
    });
  }

  async deleteContractItems(contractId: string): Promise<void> {
    await db
      .delete(schema.contractItems)
      .where(eq(schema.contractItems.contractId, contractId));
  }

  // Contract Installments
  async createContractInstallment(installment: InsertContractInstallment): Promise<ContractInstallment> {
    const [newInstallment] = await db.insert(schema.contractInstallments).values(installment).returning();
    return newInstallment;
  }

  async createContractInstallmentsBatch(installments: InsertContractInstallment[]): Promise<ContractInstallment[]> {
    if (installments.length === 0) return [];
    const newInstallments = await db.insert(schema.contractInstallments).values(installments).returning();
    return newInstallments;
  }

  async getContractInstallments(contractId: string): Promise<ContractInstallment[]> {
    return await db.query.contractInstallments.findMany({
      where: eq(schema.contractInstallments.contractId, contractId),
      orderBy: [schema.contractInstallments.sequence],
    });
  }

  async updateContractInstallment(id: string, data: Partial<InsertContractInstallment>): Promise<ContractInstallment | undefined> {
    const [updated] = await db
      .update(schema.contractInstallments)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.contractInstallments.id, id))
      .returning();
    return updated;
  }

  async deleteContractInstallments(contractId: string): Promise<void> {
    await db
      .delete(schema.contractInstallments)
      .where(eq(schema.contractInstallments.contractId, contractId));
  }

  // Lead Workflows
  async createLeadWorkflow(workflow: InsertLeadWorkflow): Promise<LeadWorkflow> {
    const [newWorkflow] = await db.insert(schema.leadWorkflows).values(workflow).returning();
    return newWorkflow;
  }

  async getLeadWorkflowsByWorkspace(workspaceId: string): Promise<LeadWorkflow[]> {
    return await db.query.leadWorkflows.findMany({
      where: eq(schema.leadWorkflows.workspaceId, workspaceId),
      orderBy: [desc(schema.leadWorkflows.createdAt)],
    });
  }

  async getLeadWorkflowById(id: string, workspaceId: string): Promise<LeadWorkflow | undefined> {
    return await db.query.leadWorkflows.findFirst({
      where: and(eq(schema.leadWorkflows.id, id), eq(schema.leadWorkflows.workspaceId, workspaceId)),
    });
  }

  async updateLeadWorkflow(id: string, workspaceId: string, data: Partial<InsertLeadWorkflow>): Promise<LeadWorkflow | undefined> {
    const [updated] = await db
      .update(schema.leadWorkflows)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.leadWorkflows.id, id), eq(schema.leadWorkflows.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteLeadWorkflow(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.leadWorkflows)
      .where(and(eq(schema.leadWorkflows.id, id), eq(schema.leadWorkflows.workspaceId, workspaceId)));
  }

  // Lead Workflow Stages
  async createLeadWorkflowStage(stage: InsertLeadWorkflowStage): Promise<LeadWorkflowStage> {
    const [newStage] = await db.insert(schema.leadWorkflowStages).values(stage).returning();
    return newStage;
  }

  async getLeadWorkflowStages(workflowId: string): Promise<LeadWorkflowStage[]> {
    return await db.query.leadWorkflowStages.findMany({
      where: eq(schema.leadWorkflowStages.workflowId, workflowId),
      orderBy: [schema.leadWorkflowStages.position],
    });
  }

  async getLeadWorkflowStageById(id: string): Promise<LeadWorkflowStage | undefined> {
    return await db.query.leadWorkflowStages.findFirst({
      where: eq(schema.leadWorkflowStages.id, id),
    });
  }

  async updateLeadWorkflowStage(id: string, data: Partial<InsertLeadWorkflowStage>): Promise<LeadWorkflowStage | undefined> {
    const [updated] = await db
      .update(schema.leadWorkflowStages)
      .set(data)
      .where(eq(schema.leadWorkflowStages.id, id))
      .returning();
    return updated;
  }

  async deleteLeadWorkflowStage(id: string): Promise<void> {
    await db.delete(schema.leadWorkflowStages).where(eq(schema.leadWorkflowStages.id, id));
  }

  async reorderLeadWorkflowStages(workflowId: string, stageIds: string[]): Promise<void> {
    for (let i = 0; i < stageIds.length; i++) {
      await db
        .update(schema.leadWorkflowStages)
        .set({ position: i })
        .where(and(eq(schema.leadWorkflowStages.id, stageIds[i]), eq(schema.leadWorkflowStages.workflowId, workflowId)));
    }
  }

  // Lead Forms
  async createLeadForm(form: InsertLeadForm): Promise<LeadForm> {
    const [newForm] = await db.insert(schema.leadForms).values(form).returning();
    return newForm;
  }

  async getLeadFormsByWorkspace(workspaceId: string): Promise<LeadForm[]> {
    return await db.query.leadForms.findMany({
      where: eq(schema.leadForms.workspaceId, workspaceId),
      orderBy: [desc(schema.leadForms.createdAt)],
    });
  }

  async getLeadFormById(id: string, workspaceId: string): Promise<LeadForm | undefined> {
    return await db.query.leadForms.findFirst({
      where: and(eq(schema.leadForms.id, id), eq(schema.leadForms.workspaceId, workspaceId)),
    });
  }

  async updateLeadForm(id: string, workspaceId: string, data: Partial<InsertLeadForm>): Promise<LeadForm | undefined> {
    const [updated] = await db
      .update(schema.leadForms)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.leadForms.id, id), eq(schema.leadForms.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteLeadForm(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.leadForms)
      .where(and(eq(schema.leadForms.id, id), eq(schema.leadForms.workspaceId, workspaceId)));
  }

  // Lead Form Fields
  async createLeadFormField(field: InsertLeadFormField): Promise<LeadFormField> {
    const [newField] = await db.insert(schema.leadFormFields).values(field).returning();
    return newField;
  }

  async getLeadFormFields(formId: string): Promise<LeadFormField[]> {
    return await db.query.leadFormFields.findMany({
      where: eq(schema.leadFormFields.formId, formId),
      orderBy: [schema.leadFormFields.position],
    });
  }

  async updateLeadFormField(id: string, data: Partial<InsertLeadFormField>): Promise<LeadFormField | undefined> {
    const [updated] = await db
      .update(schema.leadFormFields)
      .set(data)
      .where(eq(schema.leadFormFields.id, id))
      .returning();
    return updated;
  }

  async deleteLeadFormField(id: string): Promise<void> {
    await db.delete(schema.leadFormFields).where(eq(schema.leadFormFields.id, id));
  }

  async reorderLeadFormFields(formId: string, fieldIds: string[]): Promise<void> {
    for (let i = 0; i < fieldIds.length; i++) {
      await db
        .update(schema.leadFormFields)
        .set({ position: i })
        .where(and(eq(schema.leadFormFields.id, fieldIds[i]), eq(schema.leadFormFields.formId, formId)));
    }
  }

  // Lead Landing Pages
  async createLeadLandingPage(page: InsertLeadLandingPage): Promise<LeadLandingPage> {
    const [newPage] = await db.insert(schema.leadLandingPages).values(page).returning();
    return newPage;
  }

  async getLeadLandingPagesByWorkspace(workspaceId: string): Promise<LeadLandingPage[]> {
    return await db.query.leadLandingPages.findMany({
      where: eq(schema.leadLandingPages.workspaceId, workspaceId),
      orderBy: [desc(schema.leadLandingPages.createdAt)],
    });
  }

  async getLeadLandingPageById(id: string, workspaceId: string): Promise<LeadLandingPage | undefined> {
    return await db.query.leadLandingPages.findFirst({
      where: and(eq(schema.leadLandingPages.id, id), eq(schema.leadLandingPages.workspaceId, workspaceId)),
    });
  }

  async getLeadLandingPageBySlug(slug: string): Promise<LeadLandingPage | undefined> {
    return await db.query.leadLandingPages.findFirst({
      where: eq(schema.leadLandingPages.slug, slug),
    });
  }

  async updateLeadLandingPage(id: string, workspaceId: string, data: Partial<InsertLeadLandingPage>): Promise<LeadLandingPage | undefined> {
    const [updated] = await db
      .update(schema.leadLandingPages)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.leadLandingPages.id, id), eq(schema.leadLandingPages.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteLeadLandingPage(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.leadLandingPages)
      .where(and(eq(schema.leadLandingPages.id, id), eq(schema.leadLandingPages.workspaceId, workspaceId)));
  }

  // Leads
  async createLead(lead: InsertLead): Promise<Lead> {
    const [newLead] = await db.insert(schema.leads).values(lead).returning();
    return newLead;
  }

  async getLeadsByWorkspace(workspaceId: string, filters?: { workflowId?: string; stageId?: string }): Promise<Lead[]> {
    let conditions = [eq(schema.leads.workspaceId, workspaceId)];
    if (filters?.workflowId) {
      conditions.push(eq(schema.leads.workflowId, filters.workflowId));
    }
    if (filters?.stageId) {
      conditions.push(eq(schema.leads.stageId, filters.stageId));
    }
    return await db.query.leads.findMany({
      where: and(...conditions),
      orderBy: [desc(schema.leads.createdAt)],
    });
  }

  async getLeadsByWorkspacePaginated(
    workspaceId: string, 
    options: { 
      page?: number; 
      pageSize?: number; 
      search?: string;
      source?: string;
      status?: string;
      tagIds?: string[];
      workflowId?: string; 
      stageId?: string;
      sortBy?: string;
    } = {}
  ): Promise<{ items: Lead[]; total: number; page: number; pageSize: number }> {
    const { page = 1, pageSize = 20, search, source, status, tagIds, workflowId, stageId, sortBy = 'newest' } = options;
    const offset = (page - 1) * pageSize;
    
    let conditions: any[] = [eq(schema.leads.workspaceId, workspaceId)];
    
    if (workflowId) {
      conditions.push(eq(schema.leads.workflowId, workflowId));
    }
    if (stageId) {
      conditions.push(eq(schema.leads.stageId, stageId));
    }
    if (source) {
      conditions.push(eq(schema.leads.source, source as any));
    }
    if (status) {
      conditions.push(eq(schema.leads.status, status as any));
    }
    if (search) {
      conditions.push(
        or(
          ilike(schema.leads.name, `%${search}%`),
          ilike(schema.leads.email, `%${search}%`),
          ilike(schema.leads.phone, `%${search}%`)
        )
      );
    }
    
    // If filtering by tags, get lead IDs that have those tags
    let leadIdsWithTags: string[] | undefined;
    if (tagIds && tagIds.length > 0) {
      const assignments = await db.query.leadTagAssignments.findMany({
        where: inArray(schema.leadTagAssignments.tagId, tagIds),
      });
      leadIdsWithTags = Array.from(new Set(assignments.map(a => a.leadId)));
      if (leadIdsWithTags.length === 0) {
        return { items: [], total: 0, page, pageSize };
      }
      conditions.push(inArray(schema.leads.id, leadIdsWithTags));
    }
    
    const whereClause = and(...conditions);
    
    // Get total count
    const countResult = await db.select({ count: sql<number>`count(*)` })
      .from(schema.leads)
      .where(whereClause);
    const total = Number(countResult[0]?.count || 0);
    
    // Determine sort order
    let orderByClause: any[];
    switch (sortBy) {
      case 'oldest':
        orderByClause = [asc(schema.leads.createdAt)];
        break;
      case 'name_asc':
        orderByClause = [asc(schema.leads.name)];
        break;
      case 'name_desc':
        orderByClause = [desc(schema.leads.name)];
        break;
      case 'updated':
        orderByClause = [desc(schema.leads.updatedAt)];
        break;
      case 'newest':
      default:
        orderByClause = [desc(schema.leads.createdAt)];
        break;
    }
    
    // Get paginated items
    const items = await db.query.leads.findMany({
      where: whereClause,
      orderBy: orderByClause,
      limit: pageSize,
      offset,
    });
    
    return { items, total, page, pageSize };
  }

  async getLeadById(id: string, workspaceId: string): Promise<Lead | undefined> {
    return await db.query.leads.findFirst({
      where: and(eq(schema.leads.id, id), eq(schema.leads.workspaceId, workspaceId)),
    });
  }

  async updateLead(id: string, workspaceId: string, data: Partial<InsertLead>): Promise<Lead | undefined> {
    const [updated] = await db
      .update(schema.leads)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.leads.id, id), eq(schema.leads.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteLead(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.leads)
      .where(and(eq(schema.leads.id, id), eq(schema.leads.workspaceId, workspaceId)));
  }

  async getLeadsWithoutContact(workspaceId: string): Promise<Lead[]> {
    return await db.query.leads.findMany({
      where: and(
        eq(schema.leads.workspaceId, workspaceId),
        or(
          isNull(schema.leads.email),
          eq(schema.leads.email, '')
        ),
        or(
          isNull(schema.leads.phone),
          eq(schema.leads.phone, '')
        )
      ),
      orderBy: [desc(schema.leads.createdAt)],
    });
  }

  async getDuplicateLeads(workspaceId: string): Promise<{ email: string | null; phone: string | null; leads: Lead[] }[]> {
    const allLeads = await db.query.leads.findMany({
      where: eq(schema.leads.workspaceId, workspaceId),
      orderBy: [desc(schema.leads.createdAt)],
    });

    const emailGroups = new Map<string, Set<string>>();
    const phoneGroups = new Map<string, Set<string>>();
    const leadMap = new Map<string, Lead>();

    for (const lead of allLeads) {
      leadMap.set(lead.id, lead);
      const normalizedEmail = lead.email?.toLowerCase().trim() || null;
      const normalizedPhone = lead.phone?.replace(/\D/g, '') || null;

      if (normalizedEmail) {
        if (!emailGroups.has(normalizedEmail)) {
          emailGroups.set(normalizedEmail, new Set());
        }
        emailGroups.get(normalizedEmail)!.add(lead.id);
      }

      if (normalizedPhone && normalizedPhone.length >= 8) {
        if (!phoneGroups.has(normalizedPhone)) {
          phoneGroups.set(normalizedPhone, new Set());
        }
        phoneGroups.get(normalizedPhone)!.add(lead.id);
      }
    }

    const processedIds = new Set<string>();
    const duplicateGroups: { email: string | null; phone: string | null; leads: Lead[] }[] = [];

    const findConnectedLeads = (startId: string): Set<string> => {
      const connected = new Set<string>();
      const queue = [startId];
      
      while (queue.length > 0) {
        const currentId = queue.shift()!;
        if (connected.has(currentId)) continue;
        connected.add(currentId);
        
        const lead = leadMap.get(currentId)!;
        const email = lead.email?.toLowerCase().trim() || null;
        const phone = lead.phone?.replace(/\D/g, '') || null;
        
        if (email && emailGroups.has(email)) {
          for (const id of Array.from(emailGroups.get(email)!)) {
            if (!connected.has(id)) queue.push(id);
          }
        }
        if (phone && phone.length >= 8 && phoneGroups.has(phone)) {
          for (const id of Array.from(phoneGroups.get(phone)!)) {
            if (!connected.has(id)) queue.push(id);
          }
        }
      }
      return connected;
    };

    for (const lead of allLeads) {
      if (processedIds.has(lead.id)) continue;
      
      const connected = findConnectedLeads(lead.id);
      if (connected.size > 1) {
        const leads = Array.from(connected).map(id => leadMap.get(id)!);
        leads.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        
        const firstLead = leads[0];
        duplicateGroups.push({
          email: firstLead.email?.toLowerCase().trim() || null,
          phone: firstLead.phone?.replace(/\D/g, '') || null,
          leads,
        });
        
        for (const id of Array.from(connected)) {
          processedIds.add(id);
        }
      }
    }

    return duplicateGroups;
  }

  async deleteLeadsBatch(workspaceId: string, leadIds: string[]): Promise<number> {
    if (leadIds.length === 0) return 0;
    
    await db.delete(schema.leadTagAssignments)
      .where(inArray(schema.leadTagAssignments.leadId, leadIds));
    
    const result = await db
      .delete(schema.leads)
      .where(and(
        eq(schema.leads.workspaceId, workspaceId),
        inArray(schema.leads.id, leadIds)
      ))
      .returning({ id: schema.leads.id });
    
    return result.length;
  }

  async incrementLeadReferralCount(referralCode: string): Promise<void> {
    await db
      .update(schema.leads)
      .set({ referralCount: sql`${schema.leads.referralCount} + 1` })
      .where(eq(schema.leads.referralCode, referralCode));
  }

  // Lead Form Submissions
  async createLeadFormSubmission(submission: InsertLeadFormSubmission): Promise<LeadFormSubmission> {
    const [newSubmission] = await db.insert(schema.leadFormSubmissions).values(submission).returning();
    return newSubmission;
  }

  async getLeadFormSubmissions(leadId: string): Promise<LeadFormSubmission[]> {
    return await db.query.leadFormSubmissions.findMany({
      where: eq(schema.leadFormSubmissions.leadId, leadId),
      orderBy: [desc(schema.leadFormSubmissions.createdAt)],
    });
  }

  // Lead Stage History
  async createLeadStageHistory(history: InsertLeadStageHistory): Promise<LeadStageHistory> {
    const [newHistory] = await db.insert(schema.leadStageHistory).values(history).returning();
    return newHistory;
  }

  async getLeadStageHistory(leadId: string): Promise<LeadStageHistory[]> {
    return await db.query.leadStageHistory.findMany({
      where: eq(schema.leadStageHistory.leadId, leadId),
      orderBy: [desc(schema.leadStageHistory.createdAt)],
    });
  }

  // Lead Tags
  async createLeadTag(tag: InsertLeadTag): Promise<LeadTag> {
    const [newTag] = await db.insert(schema.leadTags).values(tag).returning();
    return newTag;
  }

  async getLeadTagsByWorkspace(workspaceId: string): Promise<LeadTag[]> {
    return await db.query.leadTags.findMany({
      where: eq(schema.leadTags.workspaceId, workspaceId),
      orderBy: [schema.leadTags.name],
    });
  }

  async getLeadTagById(id: string, workspaceId: string): Promise<LeadTag | undefined> {
    return await db.query.leadTags.findFirst({
      where: and(eq(schema.leadTags.id, id), eq(schema.leadTags.workspaceId, workspaceId)),
    });
  }

  async updateLeadTag(id: string, workspaceId: string, data: Partial<InsertLeadTag>): Promise<LeadTag | undefined> {
    const [updated] = await db
      .update(schema.leadTags)
      .set(data)
      .where(and(eq(schema.leadTags.id, id), eq(schema.leadTags.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteLeadTag(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.leadTags)
      .where(and(eq(schema.leadTags.id, id), eq(schema.leadTags.workspaceId, workspaceId)));
  }

  // Lead Tag Assignments
  async getLeadTagAssignments(leadId: string): Promise<LeadTag[]> {
    const assignments = await db.query.leadTagAssignments.findMany({
      where: eq(schema.leadTagAssignments.leadId, leadId),
    });
    if (assignments.length === 0) return [];
    const tagIds = assignments.map(a => a.tagId);
    return await db.query.leadTags.findMany({
      where: inArray(schema.leadTags.id, tagIds),
    });
  }

  async setLeadTags(leadId: string, tagIds: string[]): Promise<void> {
    // Remove existing assignments
    await db.delete(schema.leadTagAssignments)
      .where(eq(schema.leadTagAssignments.leadId, leadId));
    
    // Add new assignments
    if (tagIds.length > 0) {
      await db.insert(schema.leadTagAssignments)
        .values(tagIds.map(tagId => ({ leadId, tagId })));
    }
  }

  async addLeadTag(leadId: string, tagId: string): Promise<void> {
    await db.insert(schema.leadTagAssignments)
      .values({ leadId, tagId })
      .onConflictDoNothing();
  }

  async removeLeadTag(leadId: string, tagId: string): Promise<void> {
    await db.delete(schema.leadTagAssignments)
      .where(and(
        eq(schema.leadTagAssignments.leadId, leadId),
        eq(schema.leadTagAssignments.tagId, tagId)
      ));
  }

  // Custom Field Definitions
  async createCustomFieldDefinition(definition: InsertCustomFieldDefinition): Promise<CustomFieldDefinition> {
    const [newDefinition] = await db.insert(schema.customFieldDefinitions).values(definition).returning();
    return newDefinition;
  }

  async getCustomFieldDefinitionsByWorkspace(workspaceId: string, entityType?: string): Promise<CustomFieldDefinition[]> {
    const conditions = [eq(schema.customFieldDefinitions.workspaceId, workspaceId)];
    if (entityType) {
      conditions.push(eq(schema.customFieldDefinitions.entityType, entityType as any));
    }
    return await db.query.customFieldDefinitions.findMany({
      where: and(...conditions),
      orderBy: [schema.customFieldDefinitions.position],
    });
  }

  async getCustomFieldDefinitionById(id: string, workspaceId: string): Promise<CustomFieldDefinition | undefined> {
    return await db.query.customFieldDefinitions.findFirst({
      where: and(eq(schema.customFieldDefinitions.id, id), eq(schema.customFieldDefinitions.workspaceId, workspaceId)),
    });
  }

  async updateCustomFieldDefinition(id: string, workspaceId: string, data: Partial<InsertCustomFieldDefinition>): Promise<CustomFieldDefinition | undefined> {
    const [updated] = await db
      .update(schema.customFieldDefinitions)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.customFieldDefinitions.id, id), eq(schema.customFieldDefinitions.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteCustomFieldDefinition(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.customFieldDefinitions)
      .where(and(eq(schema.customFieldDefinitions.id, id), eq(schema.customFieldDefinitions.workspaceId, workspaceId)));
  }

  async reorderCustomFieldDefinitions(workspaceId: string, entityType: string, definitionIds: string[]): Promise<void> {
    for (let i = 0; i < definitionIds.length; i++) {
      await db
        .update(schema.customFieldDefinitions)
        .set({ position: i })
        .where(and(
          eq(schema.customFieldDefinitions.id, definitionIds[i]),
          eq(schema.customFieldDefinitions.workspaceId, workspaceId),
          eq(schema.customFieldDefinitions.entityType, entityType as any)
        ));
    }
  }

  // Custom Field Values
  async getCustomFieldValues(workspaceId: string, entityType: string, entityId: string): Promise<CustomFieldValue[]> {
    return await db.query.customFieldValues.findMany({
      where: and(
        eq(schema.customFieldValues.workspaceId, workspaceId),
        eq(schema.customFieldValues.entityType, entityType as any),
        eq(schema.customFieldValues.entityId, entityId)
      ),
    });
  }

  async upsertCustomFieldValues(workspaceId: string, entityType: string, entityId: string, values: { definitionId: string; value: any }[]): Promise<void> {
    for (const { definitionId, value } of values) {
      const existing = await db.query.customFieldValues.findFirst({
        where: and(
          eq(schema.customFieldValues.workspaceId, workspaceId),
          eq(schema.customFieldValues.entityType, entityType as any),
          eq(schema.customFieldValues.entityId, entityId),
          eq(schema.customFieldValues.definitionId, definitionId)
        ),
      });

      const definition = await this.getCustomFieldDefinitionById(definitionId, workspaceId);
      if (!definition) continue;

      const valueData: Partial<InsertCustomFieldValue> = {
        workspaceId,
        entityType: entityType as any,
        entityId,
        definitionId,
        valueText: null,
        valueNumber: null,
        valueDate: null,
        valueBoolean: null,
        valueJson: null,
      };

      switch (definition.fieldType) {
        case 'TEXT':
        case 'TEXTAREA':
        case 'URL':
        case 'EMAIL':
        case 'PHONE':
        case 'SELECT':
          valueData.valueText = value?.toString() || null;
          break;
        case 'NUMBER':
        case 'CURRENCY':
          valueData.valueNumber = value ? String(parseFloat(value)) : null;
          break;
        case 'DATE':
          valueData.valueDate = value ? new Date(value) : null;
          break;
        case 'CHECKBOX':
          valueData.valueBoolean = Boolean(value);
          break;
        case 'MULTISELECT':
          valueData.valueJson = value ? JSON.stringify(value) : null;
          break;
      }

      if (existing) {
        await db
          .update(schema.customFieldValues)
          .set({ ...valueData, updatedAt: new Date() })
          .where(eq(schema.customFieldValues.id, existing.id));
      } else {
        await db.insert(schema.customFieldValues).values(valueData as InsertCustomFieldValue);
      }
    }
  }

  async deleteCustomFieldValues(entityType: string, entityId: string): Promise<void> {
    await db
      .delete(schema.customFieldValues)
      .where(and(
        eq(schema.customFieldValues.entityType, entityType as any),
        eq(schema.customFieldValues.entityId, entityId)
      ));
  }

  // Scheduler Types
  async createSchedulerType(schedulerType: InsertSchedulerType): Promise<SchedulerType> {
    const [newType] = await db.insert(schema.schedulerTypes).values(schedulerType).returning();
    return newType;
  }

  async getSchedulerTypesByWorkspace(workspaceId: string): Promise<SchedulerType[]> {
    return await db.query.schedulerTypes.findMany({
      where: eq(schema.schedulerTypes.workspaceId, workspaceId),
      orderBy: desc(schema.schedulerTypes.createdAt),
    });
  }

  async getSchedulerTypeById(id: string, workspaceId: string): Promise<SchedulerType | undefined> {
    return await db.query.schedulerTypes.findFirst({
      where: and(
        eq(schema.schedulerTypes.id, id),
        eq(schema.schedulerTypes.workspaceId, workspaceId)
      ),
    });
  }

  async getSchedulerTypeBySlug(slug: string, workspaceId: string): Promise<SchedulerType | undefined> {
    return await db.query.schedulerTypes.findFirst({
      where: and(
        eq(schema.schedulerTypes.slug, slug),
        eq(schema.schedulerTypes.workspaceId, workspaceId)
      ),
    });
  }

  async updateSchedulerType(id: string, workspaceId: string, data: Partial<InsertSchedulerType>): Promise<SchedulerType | undefined> {
    const [updated] = await db
      .update(schema.schedulerTypes)
      .set(data)
      .where(and(
        eq(schema.schedulerTypes.id, id),
        eq(schema.schedulerTypes.workspaceId, workspaceId)
      ))
      .returning();
    return updated;
  }

  async deleteSchedulerType(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.schedulerTypes)
      .where(and(
        eq(schema.schedulerTypes.id, id),
        eq(schema.schedulerTypes.workspaceId, workspaceId)
      ));
  }

  // Scheduler Availability
  async createSchedulerAvailability(availability: InsertSchedulerAvailability): Promise<SchedulerAvailability> {
    const [newAvail] = await db.insert(schema.schedulerAvailability).values(availability).returning();
    return newAvail;
  }

  async getSchedulerAvailability(schedulerTypeId: string): Promise<SchedulerAvailability[]> {
    return await db.query.schedulerAvailability.findMany({
      where: eq(schema.schedulerAvailability.schedulerTypeId, schedulerTypeId),
      orderBy: schema.schedulerAvailability.dayOfWeek,
    });
  }

  async deleteSchedulerAvailability(schedulerTypeId: string): Promise<void> {
    await db
      .delete(schema.schedulerAvailability)
      .where(eq(schema.schedulerAvailability.schedulerTypeId, schedulerTypeId));
  }

  async replaceSchedulerAvailability(schedulerTypeId: string, availability: InsertSchedulerAvailability[]): Promise<SchedulerAvailability[]> {
    return await db.transaction(async (tx) => {
      await tx.delete(schema.schedulerAvailability)
        .where(eq(schema.schedulerAvailability.schedulerTypeId, schedulerTypeId));
      if (availability.length === 0) return [];
      return await tx.insert(schema.schedulerAvailability).values(availability).returning();
    });
  }

  // Scheduler Exceptions
  async createSchedulerException(exception: InsertSchedulerException): Promise<SchedulerException> {
    const [newException] = await db.insert(schema.schedulerExceptions).values(exception).returning();
    return newException;
  }

  async getSchedulerExceptions(schedulerTypeId: string): Promise<SchedulerException[]> {
    return await db.query.schedulerExceptions.findMany({
      where: eq(schema.schedulerExceptions.schedulerTypeId, schedulerTypeId),
      orderBy: schema.schedulerExceptions.date,
    });
  }

  async deleteSchedulerException(id: string): Promise<void> {
    await db.delete(schema.schedulerExceptions).where(eq(schema.schedulerExceptions.id, id));
  }

  // Scheduler Bookings
  async createSchedulerBooking(booking: InsertSchedulerBooking): Promise<SchedulerBooking> {
    const [newBooking] = await db.insert(schema.schedulerBookings).values(booking).returning();
    return newBooking;
  }

  async getSchedulerBookingsByWorkspace(workspaceId: string): Promise<SchedulerBooking[]> {
    return await db.query.schedulerBookings.findMany({
      where: eq(schema.schedulerBookings.workspaceId, workspaceId),
      orderBy: desc(schema.schedulerBookings.startTime),
    });
  }

  async getSchedulerBookingsBySchedulerType(schedulerTypeId: string): Promise<SchedulerBooking[]> {
    return await db.query.schedulerBookings.findMany({
      where: eq(schema.schedulerBookings.schedulerTypeId, schedulerTypeId),
      orderBy: desc(schema.schedulerBookings.startTime),
    });
  }

  async getSchedulerBookingById(id: string, workspaceId: string): Promise<SchedulerBooking | undefined> {
    return await db.query.schedulerBookings.findFirst({
      where: and(
        eq(schema.schedulerBookings.id, id),
        eq(schema.schedulerBookings.workspaceId, workspaceId)
      ),
    });
  }

  async getSchedulerBookingsByDateRange(schedulerTypeId: string, startDate: Date, endDate: Date): Promise<SchedulerBooking[]> {
    return await db.query.schedulerBookings.findMany({
      where: and(
        eq(schema.schedulerBookings.schedulerTypeId, schedulerTypeId),
        gte(schema.schedulerBookings.startTime, startDate),
        lte(schema.schedulerBookings.endTime, endDate)
      ),
      orderBy: schema.schedulerBookings.startTime,
    });
  }

  async updateSchedulerBooking(id: string, workspaceId: string, data: Partial<InsertSchedulerBooking>): Promise<SchedulerBooking | undefined> {
    const [updated] = await db
      .update(schema.schedulerBookings)
      .set(data)
      .where(and(
        eq(schema.schedulerBookings.id, id),
        eq(schema.schedulerBookings.workspaceId, workspaceId)
      ))
      .returning();
    return updated;
  }

  async deleteSchedulerBooking(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.schedulerBookings)
      .where(and(
        eq(schema.schedulerBookings.id, id),
        eq(schema.schedulerBookings.workspaceId, workspaceId)
      ));
  }

  // Site Projects
  async createSiteProject(project: InsertSiteProject): Promise<SiteProject> {
    const [newProject] = await db.insert(schema.siteProjects).values(project).returning();
    return newProject;
  }

  async getSiteProjectsByWorkspace(workspaceId: string): Promise<SiteProject[]> {
    return await db.query.siteProjects.findMany({
      where: eq(schema.siteProjects.workspaceId, workspaceId),
      orderBy: [desc(schema.siteProjects.createdAt)],
    });
  }

  async getSiteProjectById(id: string, workspaceId: string): Promise<SiteProject | undefined> {
    return await db.query.siteProjects.findFirst({
      where: and(eq(schema.siteProjects.id, id), eq(schema.siteProjects.workspaceId, workspaceId)),
    });
  }

  async getSiteProjectByIdForValidation(id: string): Promise<SiteProject | undefined> {
    return await db.query.siteProjects.findFirst({
      where: eq(schema.siteProjects.id, id),
    });
  }

  async getSiteProjectBySlug(workspaceId: string, slug: string): Promise<SiteProject | undefined> {
    return await db.query.siteProjects.findFirst({
      where: and(eq(schema.siteProjects.workspaceId, workspaceId), eq(schema.siteProjects.slug, slug)),
    });
  }

  async updateSiteProject(id: string, workspaceId: string, data: Partial<InsertSiteProject>): Promise<SiteProject | undefined> {
    const [updated] = await db
      .update(schema.siteProjects)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.siteProjects.id, id), eq(schema.siteProjects.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteSiteProject(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.siteProjects)
      .where(and(eq(schema.siteProjects.id, id), eq(schema.siteProjects.workspaceId, workspaceId)));
  }

  // Site Pages
  async createSitePage(page: InsertSitePage): Promise<SitePage> {
    const [newPage] = await db.insert(schema.sitePages).values(page).returning();
    return newPage;
  }

  async getSitePagesBySite(siteId: string): Promise<SitePage[]> {
    return await db.query.sitePages.findMany({
      where: eq(schema.sitePages.siteId, siteId),
      orderBy: [schema.sitePages.navigationOrder],
    });
  }

  async getSitePageById(id: string): Promise<SitePage | undefined> {
    return await db.query.sitePages.findFirst({
      where: eq(schema.sitePages.id, id),
    });
  }

  async getSitePageBySlug(siteId: string, slug: string): Promise<SitePage | undefined> {
    return await db.query.sitePages.findFirst({
      where: and(eq(schema.sitePages.siteId, siteId), eq(schema.sitePages.slug, slug)),
    });
  }

  async updateSitePage(id: string, data: Partial<InsertSitePage>): Promise<SitePage | undefined> {
    const [updated] = await db
      .update(schema.sitePages)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.sitePages.id, id))
      .returning();
    return updated;
  }

  async deleteSitePage(id: string): Promise<void> {
    await db.delete(schema.sitePages).where(eq(schema.sitePages.id, id));
  }

  // Site Page Versions
  async createSitePageVersion(version: InsertSitePageVersion): Promise<SitePageVersion> {
    const [newVersion] = await db.insert(schema.sitePageVersions).values(version).returning();
    return newVersion;
  }

  async getSitePageVersions(pageId: string): Promise<SitePageVersion[]> {
    return await db.query.sitePageVersions.findMany({
      where: eq(schema.sitePageVersions.pageId, pageId),
      orderBy: [desc(schema.sitePageVersions.versionNumber)],
    });
  }

  // Blog Categories
  async createBlogCategory(category: InsertBlogCategory): Promise<BlogCategory> {
    const [newCategory] = await db.insert(schema.blogCategories).values(category).returning();
    return newCategory;
  }

  async getBlogCategoriesBySite(siteId: string): Promise<BlogCategory[]> {
    return await db.query.blogCategories.findMany({
      where: eq(schema.blogCategories.siteId, siteId),
      orderBy: [schema.blogCategories.name],
    });
  }

  async getBlogCategoryById(id: string): Promise<BlogCategory | undefined> {
    return await db.query.blogCategories.findFirst({
      where: eq(schema.blogCategories.id, id),
    });
  }

  async updateBlogCategory(id: string, data: Partial<InsertBlogCategory>): Promise<BlogCategory | undefined> {
    const [updated] = await db
      .update(schema.blogCategories)
      .set(data)
      .where(eq(schema.blogCategories.id, id))
      .returning();
    return updated;
  }

  async deleteBlogCategory(id: string): Promise<void> {
    await db.delete(schema.blogCategories).where(eq(schema.blogCategories.id, id));
  }

  // Blog Posts
  async createBlogPost(post: InsertBlogPost): Promise<BlogPost> {
    const [newPost] = await db.insert(schema.blogPosts).values(post).returning();
    return newPost;
  }

  async getBlogPostsBySite(siteId: string): Promise<BlogPost[]> {
    return await db.query.blogPosts.findMany({
      where: eq(schema.blogPosts.siteId, siteId),
      orderBy: [desc(schema.blogPosts.createdAt)],
    });
  }

  async getBlogPostById(id: string): Promise<BlogPost | undefined> {
    return await db.query.blogPosts.findFirst({
      where: eq(schema.blogPosts.id, id),
    });
  }

  async getBlogPostBySlug(siteId: string, slug: string): Promise<BlogPost | undefined> {
    return await db.query.blogPosts.findFirst({
      where: and(eq(schema.blogPosts.siteId, siteId), eq(schema.blogPosts.slug, slug)),
    });
  }

  async updateBlogPost(id: string, data: Partial<InsertBlogPost>): Promise<BlogPost | undefined> {
    const [updated] = await db
      .update(schema.blogPosts)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.blogPosts.id, id))
      .returning();
    return updated;
  }

  async deleteBlogPost(id: string): Promise<void> {
    await db.delete(schema.blogPosts).where(eq(schema.blogPosts.id, id));
  }

  async incrementBlogPostViewCount(id: string): Promise<void> {
    await db
      .update(schema.blogPosts)
      .set({ viewCount: sql`${schema.blogPosts.viewCount} + 1` })
      .where(eq(schema.blogPosts.id, id));
  }

  // Blog Post Versions
  async createBlogPostVersion(version: InsertBlogPostVersion): Promise<BlogPostVersion> {
    const [newVersion] = await db.insert(schema.blogPostVersions).values(version).returning();
    return newVersion;
  }

  async getBlogPostVersions(postId: string): Promise<BlogPostVersion[]> {
    return await db.query.blogPostVersions.findMany({
      where: eq(schema.blogPostVersions.postId, postId),
      orderBy: [desc(schema.blogPostVersions.versionNumber)],
    });
  }

  // Site Analytics
  async createSiteVisitEvent(event: InsertSiteVisitEvent): Promise<SiteVisitEvent> {
    const [newEvent] = await db.insert(schema.siteVisitEvents).values(event).returning();
    return newEvent;
  }

  async getSiteVisitStats(siteId: string, startDate: Date, endDate: Date): Promise<SiteVisitDailyStat[]> {
    return await db.query.siteVisitDailyStats.findMany({
      where: and(
        eq(schema.siteVisitDailyStats.siteId, siteId),
        gte(schema.siteVisitDailyStats.date, startDate.toISOString().split('T')[0]),
        lte(schema.siteVisitDailyStats.date, endDate.toISOString().split('T')[0])
      ),
      orderBy: [schema.siteVisitDailyStats.date],
    });
  }

  // Conversations
  async getConversations(workspaceId: string): Promise<Conversation[]> {
    return await db.query.conversations.findMany({
      where: eq(schema.conversations.workspaceId, workspaceId),
      orderBy: [desc(schema.conversations.lastMessageAt), desc(schema.conversations.createdAt)],
    });
  }

  async getConversation(id: string, workspaceId: string): Promise<Conversation | undefined> {
    return await db.query.conversations.findFirst({
      where: and(eq(schema.conversations.id, id), eq(schema.conversations.workspaceId, workspaceId)),
    });
  }

  async createConversation(data: InsertConversation): Promise<Conversation> {
    const [newConversation] = await db.insert(schema.conversations).values(data).returning();
    return newConversation;
  }

  async getOrCreateDirectConversation(workspaceId: string, userId1: string, userId2: string): Promise<Conversation> {
    const existingConversations = await db.query.conversations.findMany({
      where: and(
        eq(schema.conversations.workspaceId, workspaceId),
        eq(schema.conversations.type, 'DIRECT')
      ),
    });

    for (const conv of existingConversations) {
      const participants = await db.query.conversationParticipants.findMany({
        where: eq(schema.conversationParticipants.conversationId, conv.id),
      });
      const participantIds = participants.map(p => p.userId);
      if (participantIds.length === 2 && participantIds.includes(userId1) && participantIds.includes(userId2)) {
        return conv;
      }
    }

    const [newConversation] = await db.insert(schema.conversations).values({
      workspaceId,
      type: 'DIRECT',
      createdBy: userId1,
    }).returning();

    await db.insert(schema.conversationParticipants).values([
      { conversationId: newConversation.id, userId: userId1 },
      { conversationId: newConversation.id, userId: userId2 },
    ]);

    return newConversation;
  }

  // Conversation Participants
  async getConversationParticipants(conversationId: string): Promise<ConversationParticipant[]> {
    return await db.query.conversationParticipants.findMany({
      where: eq(schema.conversationParticipants.conversationId, conversationId),
    });
  }

  async addConversationParticipant(data: InsertConversationParticipant): Promise<ConversationParticipant> {
    const [newParticipant] = await db.insert(schema.conversationParticipants).values(data).returning();
    return newParticipant;
  }

  async updateParticipantLastRead(conversationId: string, userId: string): Promise<void> {
    await db
      .update(schema.conversationParticipants)
      .set({ lastReadAt: new Date() })
      .where(
        and(
          eq(schema.conversationParticipants.conversationId, conversationId),
          eq(schema.conversationParticipants.userId, userId)
        )
      );
  }

  // Messages
  async getMessages(conversationId: string, limit: number = 50, offset: number = 0): Promise<Message[]> {
    return await db.query.messages.findMany({
      where: eq(schema.messages.conversationId, conversationId),
      orderBy: [desc(schema.messages.createdAt)],
      limit,
      offset,
    });
  }

  async getMessage(id: string): Promise<Message | undefined> {
    return await db.query.messages.findFirst({
      where: eq(schema.messages.id, id),
    });
  }

  async createMessage(data: InsertMessage): Promise<Message> {
    const [newMessage] = await db.insert(schema.messages).values(data).returning();
    await db
      .update(schema.conversations)
      .set({ lastMessageAt: new Date() })
      .where(eq(schema.conversations.id, data.conversationId));
    return newMessage;
  }

  async updateMessage(id: string, content: string): Promise<Message | undefined> {
    const [updated] = await db
      .update(schema.messages)
      .set({ content, isEdited: true, editedAt: new Date() })
      .where(eq(schema.messages.id, id))
      .returning();
    return updated;
  }

  async deleteMessage(id: string): Promise<void> {
    await db.delete(schema.messages).where(eq(schema.messages.id, id));
  }

  // Comments
  async getComments(workspaceId: string, entityType: string, entityId: string): Promise<Comment[]> {
    return await db.query.comments.findMany({
      where: and(
        eq(schema.comments.workspaceId, workspaceId),
        eq(schema.comments.entityType, entityType as any),
        eq(schema.comments.entityId, entityId)
      ),
      orderBy: [schema.comments.createdAt],
    });
  }

  async createComment(data: InsertComment): Promise<Comment> {
    const [newComment] = await db.insert(schema.comments).values(data).returning();
    return newComment;
  }

  async updateComment(id: string, content: string): Promise<Comment | undefined> {
    const [updated] = await db
      .update(schema.comments)
      .set({ content, updatedAt: new Date() })
      .where(eq(schema.comments.id, id))
      .returning();
    return updated;
  }

  async deleteComment(id: string): Promise<void> {
    await db.delete(schema.comments).where(eq(schema.comments.id, id));
  }

  // Files
  async getFiles(workspaceId: string, options?: { projectId?: string; clientId?: string; taskId?: string; folder?: string }): Promise<File[]> {
    let conditions = [eq(schema.files.workspaceId, workspaceId)];
    
    if (options?.projectId) {
      conditions.push(eq(schema.files.projectId, options.projectId));
    }
    if (options?.clientId) {
      conditions.push(eq(schema.files.clientId, options.clientId));
    }
    if (options?.taskId) {
      conditions.push(eq(schema.files.taskId, options.taskId));
    }
    if (options?.folder) {
      conditions.push(eq(schema.files.folder, options.folder));
    }

    return await db.query.files.findMany({
      where: and(...conditions),
      orderBy: [desc(schema.files.createdAt)],
    });
  }

  async getFile(id: string, workspaceId: string): Promise<File | undefined> {
    return await db.query.files.findFirst({
      where: and(eq(schema.files.id, id), eq(schema.files.workspaceId, workspaceId)),
    });
  }

  async createFile(data: InsertFile): Promise<File> {
    const [newFile] = await db.insert(schema.files).values(data).returning();
    return newFile;
  }

  async deleteFile(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.files)
      .where(and(eq(schema.files.id, id), eq(schema.files.workspaceId, workspaceId)));
  }

  // Client Documents
  async getClientDocuments(workspaceId: string, clientId: string): Promise<ClientDocument[]> {
    return await db.query.clientDocuments.findMany({
      where: and(
        eq(schema.clientDocuments.workspaceId, workspaceId),
        eq(schema.clientDocuments.clientId, clientId)
      ),
      orderBy: [desc(schema.clientDocuments.createdAt)],
    });
  }

  async getExpiringDocuments(workspaceId: string, daysAhead: number): Promise<ClientDocument[]> {
    const futureDate = new Date();
    futureDate.setDate(futureDate.getDate() + daysAhead);
    const futureDateStr = futureDate.toISOString().split('T')[0];
    const todayStr = new Date().toISOString().split('T')[0];

    return await db.query.clientDocuments.findMany({
      where: and(
        eq(schema.clientDocuments.workspaceId, workspaceId),
        gte(schema.clientDocuments.expirationDate, todayStr),
        lte(schema.clientDocuments.expirationDate, futureDateStr)
      ),
      orderBy: [schema.clientDocuments.expirationDate],
    });
  }

  async getClientDocument(id: string, workspaceId: string): Promise<ClientDocument | undefined> {
    return await db.query.clientDocuments.findFirst({
      where: and(eq(schema.clientDocuments.id, id), eq(schema.clientDocuments.workspaceId, workspaceId)),
    });
  }

  async createClientDocument(data: InsertClientDocument): Promise<ClientDocument> {
    const [newDocument] = await db.insert(schema.clientDocuments).values(data).returning();
    return newDocument;
  }

  async updateClientDocument(id: string, data: Partial<InsertClientDocument>): Promise<ClientDocument | undefined> {
    const [updated] = await db
      .update(schema.clientDocuments)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.clientDocuments.id, id))
      .returning();
    return updated;
  }

  async deleteClientDocument(id: string, workspaceId: string): Promise<void> {
    await db
      .delete(schema.clientDocuments)
      .where(and(eq(schema.clientDocuments.id, id), eq(schema.clientDocuments.workspaceId, workspaceId)));
  }

  // Notifications
  async getNotifications(workspaceId: string, userId: string): Promise<Notification[]> {
    return await db.query.notifications.findMany({
      where: and(
        eq(schema.notifications.workspaceId, workspaceId),
        eq(schema.notifications.userId, userId)
      ),
      orderBy: [desc(schema.notifications.createdAt)],
    });
  }

  async getUnreadNotificationCount(workspaceId: string, userId: string): Promise<number> {
    const result = await db
      .select({ count: sql<number>`count(*)::int` })
      .from(schema.notifications)
      .where(
        and(
          eq(schema.notifications.workspaceId, workspaceId),
          eq(schema.notifications.userId, userId),
          eq(schema.notifications.isRead, false)
        )
      );
    return result[0]?.count ?? 0;
  }

  async createNotification(data: InsertNotification): Promise<Notification> {
    const [newNotification] = await db.insert(schema.notifications).values(data).returning();
    return newNotification;
  }

  async markNotificationRead(id: string): Promise<Notification | undefined> {
    const [updated] = await db
      .update(schema.notifications)
      .set({ isRead: true, readAt: new Date() })
      .where(eq(schema.notifications.id, id))
      .returning();
    return updated;
  }

  async markAllNotificationsRead(workspaceId: string, userId: string): Promise<void> {
    await db
      .update(schema.notifications)
      .set({ isRead: true, readAt: new Date() })
      .where(
        and(
          eq(schema.notifications.workspaceId, workspaceId),
          eq(schema.notifications.userId, userId),
          eq(schema.notifications.isRead, false)
        )
      );
  }

  // ==========================================
  // WORKFLOWS
  // ==========================================

  async getWorkflows(workspaceId: string): Promise<Workflow[]> {
    return await db.query.workflows.findMany({
      where: eq(schema.workflows.workspaceId, workspaceId),
      orderBy: [desc(schema.workflows.createdAt)],
    });
  }

  async getWorkflow(id: string, workspaceId: string): Promise<Workflow | undefined> {
    return await db.query.workflows.findFirst({
      where: and(eq(schema.workflows.id, id), eq(schema.workflows.workspaceId, workspaceId)),
    });
  }

  async getActiveWorkflowsByTrigger(workspaceId: string, triggerType: string): Promise<Workflow[]> {
    return await db.query.workflows.findMany({
      where: and(
        eq(schema.workflows.workspaceId, workspaceId),
        eq(schema.workflows.triggerType, triggerType as any),
        eq(schema.workflows.status, 'ACTIVE')
      ),
    });
  }

  async getWorkflowTemplates(): Promise<Workflow[]> {
    return await db.query.workflows.findMany({
      where: eq(schema.workflows.isTemplate, true),
      orderBy: [schema.workflows.templateCategory, schema.workflows.name],
    });
  }

  async createWorkflow(data: InsertWorkflow): Promise<Workflow> {
    const [workflow] = await db.insert(schema.workflows).values(data).returning();
    return workflow;
  }

  async updateWorkflow(id: string, workspaceId: string, data: Partial<InsertWorkflow>): Promise<Workflow | undefined> {
    const [updated] = await db
      .update(schema.workflows)
      .set({ ...data, updatedAt: new Date() })
      .where(and(eq(schema.workflows.id, id), eq(schema.workflows.workspaceId, workspaceId)))
      .returning();
    return updated;
  }

  async deleteWorkflow(id: string, workspaceId: string): Promise<void> {
    await db.delete(schema.workflows).where(and(eq(schema.workflows.id, id), eq(schema.workflows.workspaceId, workspaceId)));
  }

  async incrementWorkflowExecutionCount(id: string): Promise<void> {
    await db
      .update(schema.workflows)
      .set({
        executionCount: sql`${schema.workflows.executionCount} + 1`,
        lastExecutedAt: new Date(),
      })
      .where(eq(schema.workflows.id, id));
  }

  // Workflow Executions
  async getWorkflowExecutions(workflowId: string, limit = 50): Promise<WorkflowExecution[]> {
    return await db.query.workflowExecutions.findMany({
      where: eq(schema.workflowExecutions.workflowId, workflowId),
      orderBy: [desc(schema.workflowExecutions.createdAt)],
      limit,
    });
  }

  async createWorkflowExecution(data: InsertWorkflowExecution): Promise<WorkflowExecution> {
    const [execution] = await db.insert(schema.workflowExecutions).values(data).returning();
    return execution;
  }

  async updateWorkflowExecution(id: string, data: Partial<WorkflowExecution>): Promise<WorkflowExecution | undefined> {
    const [updated] = await db
      .update(schema.workflowExecutions)
      .set(data)
      .where(eq(schema.workflowExecutions.id, id))
      .returning();
    return updated;
  }

  // ==========================================
  // AI CHAT SESSIONS
  // ==========================================

  async getAiChatSessions(workspaceId: string, userId: string): Promise<AiChatSession[]> {
    return await db.query.aiChatSessions.findMany({
      where: and(
        eq(schema.aiChatSessions.workspaceId, workspaceId),
        eq(schema.aiChatSessions.userId, userId)
      ),
      orderBy: [desc(schema.aiChatSessions.updatedAt)],
    });
  }

  async getAiChatSession(id: string): Promise<AiChatSession | undefined> {
    return await db.query.aiChatSessions.findFirst({
      where: eq(schema.aiChatSessions.id, id),
    });
  }

  async createAiChatSession(data: InsertAiChatSession): Promise<AiChatSession> {
    const [session] = await db.insert(schema.aiChatSessions).values(data).returning();
    return session;
  }

  async updateAiChatSession(id: string, data: Partial<InsertAiChatSession>): Promise<AiChatSession | undefined> {
    const [updated] = await db
      .update(schema.aiChatSessions)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.aiChatSessions.id, id))
      .returning();
    return updated;
  }

  async deleteAiChatSession(id: string): Promise<void> {
    await db.delete(schema.aiChatSessions).where(eq(schema.aiChatSessions.id, id));
  }

  // AI Chat Messages
  async getAiChatMessages(sessionId: string): Promise<AiChatMessage[]> {
    return await db.query.aiChatMessages.findMany({
      where: eq(schema.aiChatMessages.sessionId, sessionId),
      orderBy: [schema.aiChatMessages.createdAt],
    });
  }

  async createAiChatMessage(data: InsertAiChatMessage): Promise<AiChatMessage> {
    const [message] = await db.insert(schema.aiChatMessages).values(data).returning();
    return message;
  }

  // ==========================================
  // HOST PANEL OPERATIONS
  // ==========================================

  // Host - Users
  async updateUserGlobalRole(userId: string, role: GlobalRole): Promise<User | undefined> {
    const [updated] = await db
      .update(schema.users)
      .set({ globalRole: role })
      .where(eq(schema.users.id, userId))
      .returning();
    return updated;
  }

  async getAllUsers(): Promise<User[]> {
    return await db.query.users.findMany({
      orderBy: [desc(schema.users.createdAt)],
    });
  }

  // Host - Workspaces
  async getAllWorkspaces(): Promise<Workspace[]> {
    return await db.query.workspaces.findMany({
      orderBy: [desc(schema.workspaces.createdAt)],
    });
  }

  async getWorkspaceWithSubscription(workspaceId: string): Promise<(Workspace & { subscription?: WorkspaceSubscription }) | undefined> {
    const workspace = await this.getWorkspaceById(workspaceId);
    if (!workspace) return undefined;
    
    const subscription = await this.getWorkspaceSubscription(workspaceId);
    return { ...workspace, subscription: subscription || undefined };
  }

  async getAllWorkspacesWithSubscriptions(): Promise<(Workspace & { subscription?: WorkspaceSubscription })[]> {
    const workspaces = await this.getAllWorkspaces();
    const result: (Workspace & { subscription?: WorkspaceSubscription })[] = [];
    
    for (const workspace of workspaces) {
      const subscription = await this.getWorkspaceSubscription(workspace.id);
      result.push({ ...workspace, subscription: subscription || undefined });
    }
    
    return result;
  }

  // Host - Subscriptions
  async getWorkspaceSubscription(workspaceId: string): Promise<WorkspaceSubscription | undefined> {
    return await db.query.workspaceSubscriptions.findFirst({
      where: eq(schema.workspaceSubscriptions.workspaceId, workspaceId),
    });
  }

  async createWorkspaceSubscription(subscription: InsertWorkspaceSubscription): Promise<WorkspaceSubscription> {
    const [newSub] = await db.insert(schema.workspaceSubscriptions).values(subscription).returning();
    return newSub;
  }

  async updateWorkspaceSubscription(workspaceId: string, data: Partial<InsertWorkspaceSubscription>): Promise<WorkspaceSubscription | undefined> {
    const [updated] = await db
      .update(schema.workspaceSubscriptions)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.workspaceSubscriptions.workspaceId, workspaceId))
      .returning();
    return updated;
  }

  // Host - Usage Metrics
  async getWorkspaceUsageMetrics(workspaceId: string, startDate: Date, endDate: Date): Promise<WorkspaceUsageMetrics[]> {
    const startStr = startDate.toISOString().split('T')[0];
    const endStr = endDate.toISOString().split('T')[0];
    return await db.query.workspaceUsageMetrics.findMany({
      where: and(
        eq(schema.workspaceUsageMetrics.workspaceId, workspaceId),
        gte(schema.workspaceUsageMetrics.date, startStr),
        lte(schema.workspaceUsageMetrics.date, endStr)
      ),
      orderBy: [schema.workspaceUsageMetrics.date],
    });
  }

  async upsertWorkspaceUsageMetrics(data: InsertWorkspaceUsageMetrics): Promise<WorkspaceUsageMetrics> {
    const existing = await db.query.workspaceUsageMetrics.findFirst({
      where: and(
        eq(schema.workspaceUsageMetrics.workspaceId, data.workspaceId),
        eq(schema.workspaceUsageMetrics.date, data.date)
      ),
    });

    if (existing) {
      const [updated] = await db
        .update(schema.workspaceUsageMetrics)
        .set(data)
        .where(eq(schema.workspaceUsageMetrics.id, existing.id))
        .returning();
      return updated;
    }

    const [created] = await db.insert(schema.workspaceUsageMetrics).values(data).returning();
    return created;
  }

  async getAggregatedUsageMetrics(): Promise<{
    totalWorkspaces: number;
    totalUsers: number;
    totalClients: number;
    totalProjects: number;
    mrr: number;
    arr: number;
    newWorkspacesThisMonth: number;
    churnedWorkspacesThisMonth: number;
    churnRate: number;
    activeUsersLast30Days: number;
    avgRevenuePerWorkspace: number;
  }> {
    const [workspacesResult] = await db.select({ count: sql<number>`count(*)` }).from(schema.workspaces);
    const [usersResult] = await db.select({ count: sql<number>`count(*)` }).from(schema.users);
    const [clientsResult] = await db.select({ count: sql<number>`count(*)` }).from(schema.clients);
    const [projectsResult] = await db.select({ count: sql<number>`count(*)` }).from(schema.projects);

    const totalWorkspaces = Number(workspacesResult?.count || 0);
    const totalUsers = Number(usersResult?.count || 0);

    const planPrices: Record<string, number> = {
      TRIAL: 0,
      STARTER: 97,
      PROFESSIONAL: 197,
      ENTERPRISE: 497,
    };

    const subscriptions = await db.query.workspaceSubscriptions.findMany({
      where: eq(schema.workspaceSubscriptions.status, 'ACTIVE'),
    });

    let mrr = 0;
    subscriptions.forEach((sub) => {
      mrr += planPrices[sub.plan] || 0;
    });

    const now = new Date();
    const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);

    const [newWorkspacesResult] = await db
      .select({ count: sql<number>`count(*)` })
      .from(schema.workspaces)
      .where(sql`${schema.workspaces.createdAt} >= ${startOfMonth}`);

    const [churnedResult] = await db
      .select({ count: sql<number>`count(*)` })
      .from(schema.workspaceSubscriptions)
      .where(
        and(
          or(
            eq(schema.workspaceSubscriptions.status, 'CANCELLED'),
            eq(schema.workspaceSubscriptions.status, 'SUSPENDED')
          ),
          sql`${schema.workspaceSubscriptions.updatedAt} >= ${startOfMonth}`
        )
      );

    const newWorkspacesThisMonth = Number(newWorkspacesResult?.count || 0);
    const churnedWorkspacesThisMonth = Number(churnedResult?.count || 0);
    
    const previousMonthTotal = totalWorkspaces - newWorkspacesThisMonth + churnedWorkspacesThisMonth;
    const churnRate = previousMonthTotal > 0 
      ? (churnedWorkspacesThisMonth / previousMonthTotal) * 100 
      : 0;

    const activeUsersLast30Days = totalUsers;
    const avgRevenuePerWorkspace = totalWorkspaces > 0 ? mrr / totalWorkspaces : 0;

    return {
      totalWorkspaces,
      totalUsers,
      totalClients: Number(clientsResult?.count || 0),
      totalProjects: Number(projectsResult?.count || 0),
      mrr,
      arr: mrr * 12,
      newWorkspacesThisMonth,
      churnedWorkspacesThisMonth,
      churnRate,
      activeUsersLast30Days,
      avgRevenuePerWorkspace,
    };
  }

  // Host - Access Events
  async createWorkspaceAccessEvent(event: InsertWorkspaceAccessEvent): Promise<WorkspaceAccessEvent> {
    const [created] = await db.insert(schema.workspaceAccessEvents).values(event).returning();
    return created;
  }

  async getWorkspaceAccessEvents(workspaceId: string, limit = 100): Promise<WorkspaceAccessEvent[]> {
    return await db.query.workspaceAccessEvents.findMany({
      where: eq(schema.workspaceAccessEvents.workspaceId, workspaceId),
      orderBy: [desc(schema.workspaceAccessEvents.createdAt)],
      limit,
    });
  }

  async getRecentAccessEvents(limit = 50): Promise<(WorkspaceAccessEvent & { workspace?: Workspace; user?: User })[]> {
    const events = await db.query.workspaceAccessEvents.findMany({
      orderBy: [desc(schema.workspaceAccessEvents.createdAt)],
      limit,
    });

    const result: (WorkspaceAccessEvent & { workspace?: Workspace; user?: User })[] = [];
    for (const event of events) {
      const workspace = await this.getWorkspaceById(event.workspaceId);
      const user = await this.getUserById(event.userId);
      result.push({ ...event, workspace: workspace || undefined, user: user || undefined });
    }

    return result;
  }

  // Landing Page Leads
  async createLandingLead(lead: InsertLandingLead): Promise<LandingLead> {
    const [created] = await db.insert(schema.landingLeads).values(lead).returning();
    return created;
  }

  async getLandingLeads(): Promise<LandingLead[]> {
    return await db.query.landingLeads.findMany({
      orderBy: [desc(schema.landingLeads.createdAt)],
    });
  }

  async updateLandingLead(id: string, data: Partial<InsertLandingLead>): Promise<LandingLead | undefined> {
    const [updated] = await db
      .update(schema.landingLeads)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.landingLeads.id, id))
      .returning();
    return updated;
  }

  // ============ VISUAL PROPOSAL BUILDER ============

  // Proposal Templates
  async createProposalTemplate(template: InsertProposalTemplate): Promise<ProposalTemplate> {
    const [created] = await db.insert(schema.proposalTemplates).values(template).returning();
    return created;
  }

  async getProposalTemplates(workspaceId?: string): Promise<ProposalTemplate[]> {
    if (workspaceId) {
      return await db.query.proposalTemplates.findMany({
        where: or(
          eq(schema.proposalTemplates.workspaceId, workspaceId),
          eq(schema.proposalTemplates.isPublic, true)
        ),
        orderBy: [desc(schema.proposalTemplates.createdAt)],
      });
    }
    return await db.query.proposalTemplates.findMany({
      where: eq(schema.proposalTemplates.isPublic, true),
      orderBy: [desc(schema.proposalTemplates.createdAt)],
    });
  }

  async getProposalTemplateById(id: string): Promise<ProposalTemplate | undefined> {
    return await db.query.proposalTemplates.findFirst({
      where: eq(schema.proposalTemplates.id, id),
    });
  }

  async updateProposalTemplate(id: string, data: Partial<InsertProposalTemplate>): Promise<ProposalTemplate | undefined> {
    const [updated] = await db
      .update(schema.proposalTemplates)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.proposalTemplates.id, id))
      .returning();
    return updated;
  }

  async deleteProposalTemplate(id: string): Promise<void> {
    await db.delete(schema.proposalTemplates).where(eq(schema.proposalTemplates.id, id));
  }

  // Proposal Block Instances
  async createProposalBlockInstance(block: InsertProposalBlockInstance): Promise<ProposalBlockInstance> {
    const [created] = await db.insert(schema.proposalBlockInstances).values(block).returning();
    return created;
  }

  async createProposalBlockInstances(blocks: InsertProposalBlockInstance[]): Promise<ProposalBlockInstance[]> {
    if (blocks.length === 0) return [];
    return await db.insert(schema.proposalBlockInstances).values(blocks).returning();
  }

  async getProposalBlockInstances(proposalId: string): Promise<ProposalBlockInstance[]> {
    return await db.query.proposalBlockInstances.findMany({
      where: eq(schema.proposalBlockInstances.proposalId, proposalId),
      orderBy: [schema.proposalBlockInstances.position],
    });
  }

  async updateProposalBlockInstance(id: string, data: Partial<InsertProposalBlockInstance>): Promise<ProposalBlockInstance | undefined> {
    const [updated] = await db
      .update(schema.proposalBlockInstances)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.proposalBlockInstances.id, id))
      .returning();
    return updated;
  }

  async deleteProposalBlockInstance(id: string): Promise<void> {
    await db.delete(schema.proposalBlockInstances).where(eq(schema.proposalBlockInstances.id, id));
  }

  async deleteAllProposalBlockInstances(proposalId: string): Promise<void> {
    await db.delete(schema.proposalBlockInstances).where(eq(schema.proposalBlockInstances.proposalId, proposalId));
  }

  async reorderProposalBlockInstances(proposalId: string, orderedIds: string[]): Promise<void> {
    for (let i = 0; i < orderedIds.length; i++) {
      await db
        .update(schema.proposalBlockInstances)
        .set({ position: i, updatedAt: new Date() })
        .where(and(
          eq(schema.proposalBlockInstances.id, orderedIds[i]),
          eq(schema.proposalBlockInstances.proposalId, proposalId)
        ));
    }
  }

  // Proposal Template Blocks (default blocks for templates)
  async createProposalTemplateBlock(block: InsertProposalTemplateBlock): Promise<ProposalTemplateBlock> {
    const [created] = await db.insert(schema.proposalTemplateBlocks).values(block).returning();
    return created;
  }

  async createProposalTemplateBlocks(blocks: InsertProposalTemplateBlock[]): Promise<ProposalTemplateBlock[]> {
    if (blocks.length === 0) return [];
    return await db.insert(schema.proposalTemplateBlocks).values(blocks).returning();
  }

  async getProposalTemplateBlocks(templateId: string): Promise<ProposalTemplateBlock[]> {
    return await db.query.proposalTemplateBlocks.findMany({
      where: eq(schema.proposalTemplateBlocks.templateId, templateId),
      orderBy: [schema.proposalTemplateBlocks.position],
    });
  }

  async deleteProposalTemplateBlocks(templateId: string): Promise<void> {
    await db.delete(schema.proposalTemplateBlocks).where(eq(schema.proposalTemplateBlocks.templateId, templateId));
  }

  async getProposalTemplateBlockById(id: string): Promise<ProposalTemplateBlock | undefined> {
    return await db.query.proposalTemplateBlocks.findFirst({
      where: eq(schema.proposalTemplateBlocks.id, id),
    });
  }

  async updateProposalTemplateBlock(id: string, templateId: string, data: Partial<InsertProposalTemplateBlock>): Promise<ProposalTemplateBlock | undefined> {
    const [updated] = await db
      .update(schema.proposalTemplateBlocks)
      .set(data)
      .where(and(
        eq(schema.proposalTemplateBlocks.id, id),
        eq(schema.proposalTemplateBlocks.templateId, templateId)
      ))
      .returning();
    return updated;
  }

  async deleteProposalTemplateBlock(id: string, templateId: string): Promise<void> {
    await db.delete(schema.proposalTemplateBlocks).where(and(
      eq(schema.proposalTemplateBlocks.id, id),
      eq(schema.proposalTemplateBlocks.templateId, templateId)
    ));
  }

  async reorderProposalTemplateBlocks(templateId: string, orderedIds: string[]): Promise<void> {
    for (let i = 0; i < orderedIds.length; i++) {
      await db
        .update(schema.proposalTemplateBlocks)
        .set({ position: i })
        .where(and(
          eq(schema.proposalTemplateBlocks.id, orderedIds[i]),
          eq(schema.proposalTemplateBlocks.templateId, templateId)
        ));
    }
  }

  // Get proposal with blocks
  async getProposalWithBlocks(proposalId: string): Promise<{ proposal: Proposal; blocks: ProposalBlockInstance[] } | undefined> {
    const proposal = await db.query.proposals.findFirst({
      where: eq(schema.proposals.id, proposalId),
    });
    if (!proposal) return undefined;

    const blocks = await this.getProposalBlockInstances(proposalId);
    return { proposal, blocks };
  }

  // ============ EMAIL LOGS ============

  async createEmailLog(data: InsertEmailLog): Promise<EmailLog> {
    const [log] = await db.insert(schema.emailLogs).values(data).returning();
    return log;
  }

  async getEmailLogById(id: string): Promise<EmailLog | undefined> {
    return await db.query.emailLogs.findFirst({
      where: eq(schema.emailLogs.id, id),
    });
  }

  async updateEmailLog(id: string, data: Partial<InsertEmailLog>): Promise<EmailLog | undefined> {
    const [updated] = await db
      .update(schema.emailLogs)
      .set(data)
      .where(eq(schema.emailLogs.id, id))
      .returning();
    return updated;
  }

  async getEmailLogsByWorkspace(workspaceId: string): Promise<EmailLog[]> {
    return await db.query.emailLogs.findMany({
      where: eq(schema.emailLogs.workspaceId, workspaceId),
      orderBy: [desc(schema.emailLogs.createdAt)],
    });
  }

  // ============ EMAIL TEMPLATES ============

  async getEmailTemplatesByWorkspace(workspaceId: string): Promise<EmailTemplate[]> {
    return await db.query.emailTemplates.findMany({
      where: or(
        eq(schema.emailTemplates.workspaceId, workspaceId),
        eq(schema.emailTemplates.isSystem, true)
      ),
      orderBy: [desc(schema.emailTemplates.createdAt)],
    });
  }

  async getEmailTemplateById(id: string): Promise<EmailTemplate | undefined> {
    return await db.query.emailTemplates.findFirst({
      where: eq(schema.emailTemplates.id, id),
    });
  }

  async createEmailTemplate(data: InsertEmailTemplate): Promise<EmailTemplate> {
    const [template] = await db.insert(schema.emailTemplates).values(data).returning();
    return template;
  }

  async updateEmailTemplate(id: string, workspaceId: string, data: Partial<InsertEmailTemplate>): Promise<EmailTemplate | undefined> {
    const [updated] = await db
      .update(schema.emailTemplates)
      .set({ ...data, updatedAt: new Date() })
      .where(and(
        eq(schema.emailTemplates.id, id),
        eq(schema.emailTemplates.workspaceId, workspaceId)
      ))
      .returning();
    return updated;
  }

  async deleteEmailTemplate(id: string, workspaceId: string): Promise<void> {
    await db.delete(schema.emailTemplates).where(and(
      eq(schema.emailTemplates.id, id),
      eq(schema.emailTemplates.workspaceId, workspaceId)
    ));
  }

  // ============ SMS TEMPLATES ============

  async getSmsTemplatesByWorkspace(workspaceId: string): Promise<SmsTemplate[]> {
    return await db.query.smsTemplates.findMany({
      where: or(
        eq(schema.smsTemplates.workspaceId, workspaceId),
        eq(schema.smsTemplates.isSystem, true)
      ),
      orderBy: [desc(schema.smsTemplates.createdAt)],
    });
  }

  async getSmsTemplateById(id: string): Promise<SmsTemplate | undefined> {
    return await db.query.smsTemplates.findFirst({
      where: eq(schema.smsTemplates.id, id),
    });
  }

  async createSmsTemplate(data: InsertSmsTemplate): Promise<SmsTemplate> {
    const [template] = await db.insert(schema.smsTemplates).values(data).returning();
    return template;
  }

  async updateSmsTemplate(id: string, workspaceId: string, data: Partial<InsertSmsTemplate>): Promise<SmsTemplate | undefined> {
    const [updated] = await db
      .update(schema.smsTemplates)
      .set({ ...data, updatedAt: new Date() })
      .where(and(
        eq(schema.smsTemplates.id, id),
        eq(schema.smsTemplates.workspaceId, workspaceId)
      ))
      .returning();
    return updated;
  }

  async deleteSmsTemplate(id: string, workspaceId: string): Promise<void> {
    await db.delete(schema.smsTemplates).where(and(
      eq(schema.smsTemplates.id, id),
      eq(schema.smsTemplates.workspaceId, workspaceId)
    ));
  }

  // ============ SMS LOGS ============

  async getSmsLogsByWorkspace(workspaceId: string): Promise<SmsLog[]> {
    return await db.query.smsLogs.findMany({
      where: eq(schema.smsLogs.workspaceId, workspaceId),
      orderBy: [desc(schema.smsLogs.createdAt)],
    });
  }

  // ============ WHATSAPP TEMPLATES ============

  async getWhatsappTemplatesByWorkspace(workspaceId: string): Promise<WhatsappTemplate[]> {
    return await db.query.whatsappTemplates.findMany({
      where: or(
        eq(schema.whatsappTemplates.workspaceId, workspaceId),
        eq(schema.whatsappTemplates.isSystem, true)
      ),
      orderBy: [desc(schema.whatsappTemplates.createdAt)],
    });
  }

  async getWhatsappTemplateById(id: string): Promise<WhatsappTemplate | undefined> {
    return await db.query.whatsappTemplates.findFirst({
      where: eq(schema.whatsappTemplates.id, id),
    });
  }

  async createWhatsappTemplate(data: InsertWhatsappTemplate): Promise<WhatsappTemplate> {
    const [template] = await db.insert(schema.whatsappTemplates).values(data).returning();
    return template;
  }

  async updateWhatsappTemplate(id: string, workspaceId: string, data: Partial<InsertWhatsappTemplate>): Promise<WhatsappTemplate | undefined> {
    const [updated] = await db
      .update(schema.whatsappTemplates)
      .set({ ...data, updatedAt: new Date() })
      .where(and(
        eq(schema.whatsappTemplates.id, id),
        eq(schema.whatsappTemplates.workspaceId, workspaceId)
      ))
      .returning();
    return updated;
  }

  async deleteWhatsappTemplate(id: string, workspaceId: string): Promise<void> {
    await db.delete(schema.whatsappTemplates).where(and(
      eq(schema.whatsappTemplates.id, id),
      eq(schema.whatsappTemplates.workspaceId, workspaceId)
    ));
  }

  // ============ WHATSAPP LOGS ============

  async getWhatsappLogsByWorkspace(workspaceId: string): Promise<WhatsappLog[]> {
    return await db.query.whatsappLogs.findMany({
      where: eq(schema.whatsappLogs.workspaceId, workspaceId),
      orderBy: [desc(schema.whatsappLogs.createdAt)],
    });
  }

  // ============ WORKSPACE GOOGLE CREDENTIALS ============

  async getWorkspaceGoogleCredentials(workspaceId: string): Promise<WorkspaceGoogleCredentials | undefined> {
    return await db.query.workspaceGoogleCredentials.findFirst({
      where: eq(schema.workspaceGoogleCredentials.workspaceId, workspaceId),
    });
  }

  async saveWorkspaceGoogleCredentials(data: InsertWorkspaceGoogleCredentials): Promise<WorkspaceGoogleCredentials> {
    const existing = await this.getWorkspaceGoogleCredentials(data.workspaceId);
    if (existing) {
      const [updated] = await db
        .update(schema.workspaceGoogleCredentials)
        .set({ ...data, updatedAt: new Date() })
        .where(eq(schema.workspaceGoogleCredentials.workspaceId, data.workspaceId))
        .returning();
      return updated;
    }
    const [created] = await db.insert(schema.workspaceGoogleCredentials).values(data).returning();
    return created;
  }

  async deleteWorkspaceGoogleCredentials(workspaceId: string): Promise<void> {
    await db.delete(schema.workspaceGoogleCredentials).where(
      eq(schema.workspaceGoogleCredentials.workspaceId, workspaceId)
    );
  }

  // ============ WORKSPACE GOOGLE CALENDAR CREDENTIALS ============

  async getWorkspaceGoogleCalendarCredentials(workspaceId: string): Promise<WorkspaceGoogleCalendarCredentials | undefined> {
    return await db.query.workspaceGoogleCalendarCredentials.findFirst({
      where: eq(schema.workspaceGoogleCalendarCredentials.workspaceId, workspaceId),
    });
  }

  async saveWorkspaceGoogleCalendarCredentials(data: InsertWorkspaceGoogleCalendarCredentials): Promise<WorkspaceGoogleCalendarCredentials> {
    const existing = await this.getWorkspaceGoogleCalendarCredentials(data.workspaceId);
    if (existing) {
      const [updated] = await db
        .update(schema.workspaceGoogleCalendarCredentials)
        .set({ ...data, updatedAt: new Date() })
        .where(eq(schema.workspaceGoogleCalendarCredentials.workspaceId, data.workspaceId))
        .returning();
      return updated;
    }
    const [created] = await db.insert(schema.workspaceGoogleCalendarCredentials).values(data).returning();
    return created;
  }

  async deleteWorkspaceGoogleCalendarCredentials(workspaceId: string): Promise<void> {
    await db.delete(schema.workspaceGoogleCalendarCredentials).where(
      eq(schema.workspaceGoogleCalendarCredentials.workspaceId, workspaceId)
    );
  }

  // ============ PROJECT RESPONSIBLES ============

  async addProjectResponsible(projectId: string, memberId: string): Promise<ProjectResponsible> {
    const [created] = await db.insert(schema.projectResponsibles).values({
      projectId,
      memberId,
    }).returning();
    return created;
  }

  async getProjectResponsibles(projectId: string): Promise<(ProjectResponsible & { member: WorkspaceMember & { user: User } })[]> {
    const responsibles = await db.query.projectResponsibles.findMany({
      where: eq(schema.projectResponsibles.projectId, projectId),
    });
    
    const result: (ProjectResponsible & { member: WorkspaceMember & { user: User } })[] = [];
    for (const r of responsibles) {
      const member = await db.query.workspaceMembers.findFirst({
        where: eq(schema.workspaceMembers.id, r.memberId),
      });
      if (member) {
        const user = await this.getUserById(member.userId);
        if (user) {
          result.push({ ...r, member: { ...member, user } });
        }
      }
    }
    return result;
  }

  async removeProjectResponsible(projectId: string, memberId: string): Promise<void> {
    await db.delete(schema.projectResponsibles).where(
      and(
        eq(schema.projectResponsibles.projectId, projectId),
        eq(schema.projectResponsibles.memberId, memberId)
      )
    );
  }

  // ============ PROJECT FOLLOWERS ============

  async addProjectFollower(projectId: string, memberId: string): Promise<ProjectFollower> {
    const [created] = await db.insert(schema.projectFollowers).values({
      projectId,
      memberId,
    }).returning();
    return created;
  }

  async getProjectFollowers(projectId: string): Promise<(ProjectFollower & { member: WorkspaceMember & { user: User } })[]> {
    const followers = await db.query.projectFollowers.findMany({
      where: eq(schema.projectFollowers.projectId, projectId),
    });
    
    const result: (ProjectFollower & { member: WorkspaceMember & { user: User } })[] = [];
    for (const f of followers) {
      const member = await db.query.workspaceMembers.findFirst({
        where: eq(schema.workspaceMembers.id, f.memberId),
      });
      if (member) {
        const user = await this.getUserById(member.userId);
        if (user) {
          result.push({ ...f, member: { ...member, user } });
        }
      }
    }
    return result;
  }

  async removeProjectFollower(projectId: string, memberId: string): Promise<void> {
    await db.delete(schema.projectFollowers).where(
      and(
        eq(schema.projectFollowers.projectId, projectId),
        eq(schema.projectFollowers.memberId, memberId)
      )
    );
  }

  // ============ PROJECT COMMUNICATIONS ============

  async createProjectCommunication(data: InsertProjectCommunication): Promise<ProjectCommunication> {
    const [created] = await db.insert(schema.projectCommunications).values(data).returning();
    return created;
  }

  async getProjectCommunications(projectId: string): Promise<ProjectCommunication[]> {
    return await db.query.projectCommunications.findMany({
      where: eq(schema.projectCommunications.projectId, projectId),
      orderBy: [desc(schema.projectCommunications.createdAt)],
    });
  }

  async getProjectCommunicationById(id: string, workspaceId: string): Promise<ProjectCommunication | undefined> {
    return await db.query.projectCommunications.findFirst({
      where: and(
        eq(schema.projectCommunications.id, id),
        eq(schema.projectCommunications.workspaceId, workspaceId)
      ),
    });
  }

  async updateProjectCommunication(id: string, workspaceId: string, data: Partial<InsertProjectCommunication>): Promise<ProjectCommunication | undefined> {
    const [updated] = await db
      .update(schema.projectCommunications)
      .set({ ...data, updatedAt: new Date() })
      .where(
        and(
          eq(schema.projectCommunications.id, id),
          eq(schema.projectCommunications.workspaceId, workspaceId)
        )
      )
      .returning();
    return updated;
  }

  async deleteProjectCommunication(id: string, workspaceId: string): Promise<void> {
    await db.delete(schema.projectCommunications).where(
      and(
        eq(schema.projectCommunications.id, id),
        eq(schema.projectCommunications.workspaceId, workspaceId)
      )
    );
  }
}

export const storage = new Storage();
