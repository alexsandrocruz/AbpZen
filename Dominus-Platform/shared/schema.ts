import { sql } from "drizzle-orm";
import { pgTable, text, varchar, timestamp, integer, decimal, boolean, pgEnum, date } from "drizzle-orm/pg-core";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";
import { z } from "zod";

// Enums
export const globalRoleEnum = pgEnum('global_role', ['USER', 'HOST']);
export const roleEnum = pgEnum('role', ['OWNER', 'MEMBER', 'CLIENT']);
export const statusEnum = pgEnum('status', ['ACTIVE', 'ARCHIVED', 'DRAFT', 'SENT', 'PAID', 'OVERDUE', 'COMPLETED', 'IN_PROGRESS', 'PENDING', 'SIGNED', 'CANCELLED']);
export const clientTypeEnum = pgEnum('client_type', ['PERSON', 'COMPANY']);
export const transactionTypeEnum = pgEnum('transaction_type', ['INCOME', 'EXPENSE']);
export const transactionStatusEnum = pgEnum('transaction_status', ['PENDING', 'RECEIVED', 'PAID', 'OVERDUE', 'CANCELLED']);
export const subscriptionPlanEnum = pgEnum('subscription_plan', ['TRIAL', 'STARTER', 'PROFESSIONAL', 'ENTERPRISE']);
export const subscriptionStatusEnum = pgEnum('subscription_status', ['ACTIVE', 'TRIAL', 'SUSPENDED', 'CANCELLED', 'EXPIRED']);

// Permission System Enums
export const memberRoleEnum = pgEnum('member_role', ['ADMIN', 'GERENTE', 'VENDEDOR', 'FINANCEIRO', 'MEMBRO']);
export const permissionModuleEnum = pgEnum('permission_module', ['CLIENTS', 'PROJECTS', 'TASKS', 'PROPOSALS', 'INVOICES', 'LEADS', 'PRODUCTS', 'TRANSACTIONS', 'SETTINGS', 'TEAM']);
export const permissionActionEnum = pgEnum('permission_action', ['VIEW', 'CREATE', 'EDIT', 'DELETE']);
export const inviteStatusEnum = pgEnum('invite_status', ['PENDING', 'ACCEPTED', 'EXPIRED', 'CANCELLED']);

// Workspaces
export const workspaces = pgTable("workspaces", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  slug: varchar("slug", { length: 100 }).notNull().unique(),
  name: text("name").notNull(),
  ownerId: varchar("owner_id").notNull(),
  defaultProposalTemplateId: varchar("default_proposal_template_id"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertWorkspaceSchema = createInsertSchema(workspaces).omit({ id: true, createdAt: true });
export type InsertWorkspace = z.infer<typeof insertWorkspaceSchema>;
export type Workspace = typeof workspaces.$inferSelect;

// Users
export const users = pgTable("users", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  email: text("email").notNull().unique(),
  password: text("password").notNull(),
  name: text("name").notNull(),
  avatar: text("avatar"),
  globalRole: globalRoleEnum("global_role").notNull().default('USER'),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertUserSchema = createInsertSchema(users).omit({ id: true, createdAt: true });
export type InsertUser = z.infer<typeof insertUserSchema>;
export type User = typeof users.$inferSelect;
export type GlobalRole = 'USER' | 'HOST';

// Workspace Members
export const workspaceMembers = pgTable("workspace_members", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  userId: varchar("user_id").notNull().references(() => users.id, { onDelete: 'cascade' }),
  role: roleEnum("role").notNull().default('MEMBER'),
  memberRole: memberRoleEnum("member_role").notNull().default('MEMBRO'),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertWorkspaceMemberSchema = createInsertSchema(workspaceMembers).omit({ id: true, createdAt: true });
export type InsertWorkspaceMember = z.infer<typeof insertWorkspaceMemberSchema>;
export type WorkspaceMember = typeof workspaceMembers.$inferSelect;

// Member Permissions (extra permissions beyond role defaults)
export const memberPermissions = pgTable("member_permissions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  memberId: varchar("member_id").notNull().references(() => workspaceMembers.id, { onDelete: 'cascade' }),
  module: permissionModuleEnum("module").notNull(),
  action: permissionActionEnum("action").notNull(),
  granted: boolean("granted").notNull().default(true),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertMemberPermissionSchema = createInsertSchema(memberPermissions).omit({ id: true, createdAt: true });
export type InsertMemberPermission = z.infer<typeof insertMemberPermissionSchema>;
export type MemberPermission = typeof memberPermissions.$inferSelect;

// Workspace Invites
export const workspaceInvites = pgTable("workspace_invites", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  email: text("email").notNull(),
  memberRole: memberRoleEnum("member_role").notNull().default('MEMBRO'),
  token: text("token").notNull().unique(),
  invitedBy: varchar("invited_by").notNull().references(() => users.id),
  status: inviteStatusEnum("status").notNull().default('PENDING'),
  expiresAt: timestamp("expires_at").notNull(),
  acceptedAt: timestamp("accepted_at"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertWorkspaceInviteSchema = createInsertSchema(workspaceInvites).omit({ id: true, createdAt: true, acceptedAt: true });
export type InsertWorkspaceInvite = z.infer<typeof insertWorkspaceInviteSchema>;
export type WorkspaceInvite = typeof workspaceInvites.$inferSelect;

// Clients (expanded with PF/PJ fields)
export const clients = pgTable("clients", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  
  // Basic info
  name: text("name").notNull(),
  email: text("email").notNull(),
  phone: text("phone"),
  
  // Type: Person or Company
  clientType: clientTypeEnum("client_type").notNull().default('COMPANY'),
  companyId: varchar("company_id"),
  
  // Company data (PJ)
  companyName: text("company_name"),
  tradeName: text("trade_name"),
  cnpj: text("cnpj"),
  stateRegistration: text("state_registration"),
  municipalRegistration: text("municipal_registration"),
  
  // Person data (PF)
  cpf: text("cpf"),
  rg: text("rg"),
  birthDate: timestamp("birth_date"),
  
  // Address
  address: text("address"),
  addressNumber: text("address_number"),
  complement: text("complement"),
  neighborhood: text("neighborhood"),
  city: text("city"),
  state: text("state"),
  zipCode: text("zip_code"),
  
  // Financial/Fiscal
  bankName: text("bank_name"),
  bankAgency: text("bank_agency"),
  bankAccount: text("bank_account"),
  pixKey: text("pix_key"),
  paymentTermDays: integer("payment_term_days").default(30),
  
  // Avatar/Logo
  avatar: text("avatar"),
  
  status: statusEnum("status").notNull().default('ACTIVE'),
  notes: text("notes"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertClientSchema = createInsertSchema(clients).omit({ id: true, createdAt: true, updatedAt: true }).extend({
  birthDate: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
});
export type InsertClient = z.infer<typeof insertClientSchema>;
export type Client = typeof clients.$inferSelect;

// Client Contacts (multiple contacts per client)
export const clientContacts = pgTable("client_contacts", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  clientId: varchar("client_id").notNull().references(() => clients.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  role: text("role"),
  email: text("email"),
  phone: text("phone"),
  whatsapp: text("whatsapp"),
  isPrimary: boolean("is_primary").notNull().default(false),
  notes: text("notes"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertClientContactSchema = createInsertSchema(clientContacts).omit({ id: true, createdAt: true });
export type InsertClientContact = z.infer<typeof insertClientContactSchema>;
export type ClientContact = typeof clientContacts.$inferSelect;

// Client Messages (Communication History)
export const messageChannelEnum = pgEnum("message_channel", ["EMAIL", "SMS", "WHATSAPP", "INSTAGRAM", "OTHER"]);
export const messageDirectionEnum = pgEnum("message_direction", ["INBOUND", "OUTBOUND"]);
export const messageStatusEnum = pgEnum("message_status", ["PENDING", "SENT", "DELIVERED", "READ", "FAILED"]);

export const clientMessages = pgTable("client_messages", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  clientId: varchar("client_id").notNull().references(() => clients.id, { onDelete: 'cascade' }),
  contactId: varchar("contact_id").references(() => clientContacts.id, { onDelete: 'set null' }),
  
  channel: messageChannelEnum("channel").notNull().default('EMAIL'),
  direction: messageDirectionEnum("direction").notNull().default('OUTBOUND'),
  status: messageStatusEnum("message_status").notNull().default('PENDING'),
  
  subject: text("subject"),
  content: text("content").notNull(),
  
  sentAt: timestamp("sent_at"),
  deliveredAt: timestamp("delivered_at"),
  readAt: timestamp("read_at"),
  
  sentBy: varchar("sent_by").references(() => users.id, { onDelete: 'set null' }),
  externalId: text("external_id"),
  metadata: text("metadata"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertClientMessageSchema = createInsertSchema(clientMessages).omit({ id: true, createdAt: true }).extend({
  sentAt: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
});
export type InsertClientMessage = z.infer<typeof insertClientMessageSchema>;
export type ClientMessage = typeof clientMessages.$inferSelect;

// Projects
export const projects = pgTable("projects", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  clientId: varchar("client_id").notNull().references(() => clients.id, { onDelete: 'cascade' }),
  title: text("title").notNull(),
  description: text("description"),
  status: statusEnum("status").notNull().default('PENDING'),
  budget: decimal("budget", { precision: 10, scale: 2 }),
  dueDate: timestamp("due_date"),
  position: integer("position").notNull().default(0),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertProjectSchema = createInsertSchema(projects).omit({ id: true, createdAt: true, updatedAt: true }).extend({
  dueDate: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
});
export type InsertProject = z.infer<typeof insertProjectSchema>;
export type Project = typeof projects.$inferSelect;

// Tasks
export const tasks = pgTable("tasks", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  projectId: varchar("project_id").references(() => projects.id, { onDelete: 'cascade' }),
  title: text("title").notNull(),
  description: text("description"),
  isCompleted: boolean("is_completed").notNull().default(false),
  assigneeId: varchar("assignee_id").references(() => users.id, { onDelete: 'set null' }),
  dueDate: timestamp("due_date"),
  position: integer("position").notNull().default(0),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertTaskSchema = createInsertSchema(tasks).omit({ id: true, createdAt: true, updatedAt: true }).extend({
  dueDate: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
});
export type InsertTask = z.infer<typeof insertTaskSchema>;
export type Task = typeof tasks.$inferSelect;

// Task Comments
export const taskComments = pgTable("task_comments", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  taskId: varchar("task_id").notNull().references(() => tasks.id, { onDelete: 'cascade' }),
  userId: varchar("user_id").notNull().references(() => users.id, { onDelete: 'cascade' }),
  content: text("content").notNull(),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertTaskCommentSchema = createInsertSchema(taskComments).omit({ id: true, createdAt: true });
export type InsertTaskComment = z.infer<typeof insertTaskCommentSchema>;
export type TaskComment = typeof taskComments.$inferSelect;

// Time Entries
export const timeEntries = pgTable("time_entries", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  userId: varchar("user_id").notNull().references(() => users.id, { onDelete: 'cascade' }),
  projectId: varchar("project_id").references(() => projects.id, { onDelete: 'set null' }),
  taskId: varchar("task_id").references(() => tasks.id, { onDelete: 'set null' }),
  description: text("description").notNull(),
  hours: decimal("hours", { precision: 5, scale: 2 }).notNull(),
  date: timestamp("date").notNull(),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertTimeEntrySchema = createInsertSchema(timeEntries).omit({ id: true, createdAt: true }).extend({
  date: z.preprocess((val) => new Date(val as string), z.date()),
});
export type InsertTimeEntry = z.infer<typeof insertTimeEntrySchema>;
export type TimeEntry = typeof timeEntries.$inferSelect;

// Financial Categories
export const financialCategories = pgTable("financial_categories", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  type: transactionTypeEnum("type").notNull(),
  color: text("color").default('#6366f1'),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertFinancialCategorySchema = createInsertSchema(financialCategories).omit({ id: true, createdAt: true });
export type InsertFinancialCategory = z.infer<typeof insertFinancialCategorySchema>;
export type FinancialCategory = typeof financialCategories.$inferSelect;

// Transactions (Income and Expense)
export const transactions = pgTable("transactions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  categoryId: varchar("category_id").references(() => financialCategories.id, { onDelete: 'set null' }),
  clientId: varchar("client_id").references(() => clients.id, { onDelete: 'set null' }),
  invoiceId: varchar("invoice_id").references(() => invoices.id, { onDelete: 'set null' }),
  
  type: transactionTypeEnum("type").notNull(),
  status: transactionStatusEnum("status").notNull().default('PENDING'),
  
  description: text("description").notNull(),
  amount: decimal("amount", { precision: 12, scale: 2 }).notNull(),
  
  dueDate: timestamp("due_date").notNull(),
  paymentDate: timestamp("payment_date"),
  
  supplier: text("supplier"),
  notes: text("notes"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertTransactionSchema = createInsertSchema(transactions).omit({ id: true, createdAt: true, updatedAt: true }).extend({
  dueDate: z.preprocess((val) => new Date(val as string), z.date()),
  paymentDate: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
});
export type InsertTransaction = z.infer<typeof insertTransactionSchema>;
export type Transaction = typeof transactions.$inferSelect;

// Transaction Attachments
export const transactionAttachments = pgTable("transaction_attachments", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  transactionId: varchar("transaction_id").notNull().references(() => transactions.id, { onDelete: 'cascade' }),
  fileName: text("file_name").notNull(),
  fileUrl: text("file_url").notNull(),
  fileType: text("file_type"),
  fileSize: integer("file_size"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertTransactionAttachmentSchema = createInsertSchema(transactionAttachments).omit({ id: true, createdAt: true });
export type InsertTransactionAttachment = z.infer<typeof insertTransactionAttachmentSchema>;
export type TransactionAttachment = typeof transactionAttachments.$inferSelect;

// Budgets (monthly budget per category)
export const budgets = pgTable("budgets", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  categoryId: varchar("category_id").notNull().references(() => financialCategories.id, { onDelete: 'cascade' }),
  year: integer("year").notNull(),
  month: integer("month").notNull(),
  amount: decimal("amount", { precision: 12, scale: 2 }).notNull(),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertBudgetSchema = createInsertSchema(budgets).omit({ id: true, createdAt: true });
export type InsertBudget = z.infer<typeof insertBudgetSchema>;
export type Budget = typeof budgets.$inferSelect;

// Product Type Enum
export const productTypeEnum = pgEnum('product_type', ['PRODUCT', 'SERVICE']);

// Products and Services Catalog
export const products = pgTable("products", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  sku: text("sku"),
  description: text("description"),
  category: text("category"),
  productType: productTypeEnum("product_type").notNull().default('PRODUCT'),
  price: decimal("price", { precision: 10, scale: 2 }).notNull(),
  unit: text("unit").default('un'),
  isActive: boolean("is_active").notNull().default(true),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertProductSchema = createInsertSchema(products).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertProduct = z.infer<typeof insertProductSchema>;
export type Product = typeof products.$inferSelect;

// Contract Templates
export const contractTemplates = pgTable("contract_templates", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  content: text("content").notNull(),
  isActive: boolean("is_active").notNull().default(true),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertContractTemplateSchema = createInsertSchema(contractTemplates).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertContractTemplate = z.infer<typeof insertContractTemplateSchema>;
export type ContractTemplate = typeof contractTemplates.$inferSelect;

// Contract Schedule Type Enum
export const contractScheduleTypeEnum = pgEnum('contract_schedule_type', ['EQUAL', 'PERCENTAGE', 'MANUAL']);
export const contractInstallmentStatusEnum = pgEnum('contract_installment_status', ['DRAFT', 'SCHEDULED', 'LINKED']);

// Contracts
export const contracts = pgTable("contracts", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  clientId: varchar("client_id").notNull().references(() => clients.id, { onDelete: 'cascade' }),
  projectId: varchar("project_id").references(() => projects.id, { onDelete: 'set null' }),
  templateId: varchar("template_id").references(() => contractTemplates.id, { onDelete: 'set null' }),
  proposalId: varchar("proposal_id").references(() => proposals.id, { onDelete: 'set null' }),
  
  title: text("title").notNull(),
  content: text("content").notNull(),
  status: statusEnum("status").notNull().default('DRAFT'),
  
  // Contract value and dates
  totalValue: decimal("total_value", { precision: 12, scale: 2 }),
  startsOn: timestamp("starts_on"),
  endsOn: timestamp("ends_on"),
  
  // Payment schedule configuration
  scheduleType: contractScheduleTypeEnum("schedule_type"),
  installmentCount: integer("installment_count"),
  installmentFrequency: text("installment_frequency"),
  installmentDueDay: integer("installment_due_day"),
  
  // Signature fields
  publicToken: varchar("public_token").unique(),
  signedAt: timestamp("signed_at"),
  signedByName: text("signed_by_name"),
  signedByIp: text("signed_by_ip"),
  signatureData: text("signature_data"),
  
  expiresAt: timestamp("expires_at"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertContractSchema = createInsertSchema(contracts).omit({ id: true, createdAt: true, updatedAt: true }).extend({
  signedAt: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
  expiresAt: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
  startsOn: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
  endsOn: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
});
export type InsertContract = z.infer<typeof insertContractSchema>;
export type Contract = typeof contracts.$inferSelect;

// Contract Items (copied from proposal)
export const contractItems = pgTable("contract_items", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  contractId: varchar("contract_id").notNull().references(() => contracts.id, { onDelete: 'cascade' }),
  productId: varchar("product_id").references(() => products.id, { onDelete: 'set null' }),
  description: text("description").notNull(),
  quantity: integer("quantity").notNull(),
  price: decimal("price", { precision: 10, scale: 2 }).notNull(),
  discount: decimal("discount", { precision: 10, scale: 2 }).default("0"),
  discountType: text("discount_type").default("PERCENT"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertContractItemSchema = createInsertSchema(contractItems).omit({ id: true, createdAt: true });
export type InsertContractItem = z.infer<typeof insertContractItemSchema>;
export type ContractItem = typeof contractItems.$inferSelect;

// Contract Installments (payment schedule)
export const contractInstallments = pgTable("contract_installments", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  contractId: varchar("contract_id").notNull().references(() => contracts.id, { onDelete: 'cascade' }),
  sequence: integer("sequence").notNull(),
  amount: decimal("amount", { precision: 12, scale: 2 }).notNull(),
  percentage: decimal("percentage", { precision: 5, scale: 2 }),
  dueDate: timestamp("due_date").notNull(),
  offsetDays: integer("offset_days"),
  description: text("description"),
  status: contractInstallmentStatusEnum("status").notNull().default('DRAFT'),
  transactionId: varchar("transaction_id").references(() => transactions.id, { onDelete: 'set null' }),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertContractInstallmentSchema = createInsertSchema(contractInstallments).omit({ id: true, createdAt: true, updatedAt: true }).extend({
  dueDate: z.preprocess((val) => new Date(val as string), z.date()),
});
export type InsertContractInstallment = z.infer<typeof insertContractInstallmentSchema>;
export type ContractInstallment = typeof contractInstallments.$inferSelect;

// Invoices
export const invoices = pgTable("invoices", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  clientId: varchar("client_id").notNull().references(() => clients.id, { onDelete: 'cascade' }),
  projectId: varchar("project_id").references(() => projects.id, { onDelete: 'set null' }),
  number: text("number").notNull(),
  issueDate: timestamp("issue_date").notNull(),
  dueDate: timestamp("due_date").notNull(),
  status: statusEnum("status").notNull().default('DRAFT'),
  notes: text("notes"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertInvoiceSchema = createInsertSchema(invoices).omit({ id: true, createdAt: true, updatedAt: true }).extend({
  issueDate: z.preprocess((val) => new Date(val as string), z.date()),
  dueDate: z.preprocess((val) => new Date(val as string), z.date()),
});
export type InsertInvoice = z.infer<typeof insertInvoiceSchema>;
export type Invoice = typeof invoices.$inferSelect;

// Invoice Items
export const invoiceItems = pgTable("invoice_items", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  invoiceId: varchar("invoice_id").notNull().references(() => invoices.id, { onDelete: 'cascade' }),
  productId: varchar("product_id").references(() => products.id, { onDelete: 'set null' }),
  description: text("description").notNull(),
  quantity: integer("quantity").notNull(),
  price: decimal("price", { precision: 10, scale: 2 }).notNull(),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertInvoiceItemSchema = createInsertSchema(invoiceItems).omit({ id: true, createdAt: true });
export type InsertInvoiceItem = z.infer<typeof insertInvoiceItemSchema>;
export type InvoiceItem = typeof invoiceItems.$inferSelect;

// Proposals
export const proposals = pgTable("proposals", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  clientId: varchar("client_id").notNull().references(() => clients.id, { onDelete: 'cascade' }),
  title: text("title").notNull(),
  content: text("content").notNull(),
  contractText: text("contract_text"),
  status: statusEnum("status").notNull().default('DRAFT'),
  
  // Visual builder settings
  useVisualBuilder: boolean("use_visual_builder").notNull().default(false),
  templateId: varchar("template_id"), // Reference to proposal_templates (no FK to avoid circular dep)
  templateStyle: text("template_style"), // JSON: cached style for rendering
  
  // Public access and signature
  publicToken: varchar("public_token").unique(),
  validUntil: timestamp("valid_until"),
  viewedAt: timestamp("viewed_at"),
  signedAt: timestamp("signed_at"),
  signedByName: text("signed_by_name"),
  signedByIp: text("signed_by_ip"),
  signatureData: text("signature_data"),
  
  // Discount
  globalDiscount: decimal("global_discount", { precision: 10, scale: 2 }).default("0"),
  globalDiscountType: text("global_discount_type").default("PERCENT"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertProposalSchema = createInsertSchema(proposals).omit({ id: true, createdAt: true, updatedAt: true }).extend({
  validUntil: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
  viewedAt: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
  signedAt: z.preprocess((val) => val ? new Date(val as string) : null, z.date().nullable().optional()),
});
export type InsertProposal = z.infer<typeof insertProposalSchema>;
export type Proposal = typeof proposals.$inferSelect;

// Proposal Line Items
export const proposalItems = pgTable("proposal_items", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  proposalId: varchar("proposal_id").notNull().references(() => proposals.id, { onDelete: 'cascade' }),
  productId: varchar("product_id").references(() => products.id, { onDelete: 'set null' }),
  description: text("description").notNull(),
  quantity: integer("quantity").notNull(),
  price: decimal("price", { precision: 10, scale: 2 }).notNull(),
  discount: decimal("discount", { precision: 10, scale: 2 }).default("0"),
  discountType: text("discount_type").default("PERCENT"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertProposalItemSchema = createInsertSchema(proposalItems).omit({ id: true, createdAt: true });
export type InsertProposalItem = z.infer<typeof insertProposalItemSchema>;
export type ProposalItem = typeof proposalItems.$inferSelect;

// ============ LEAD CAPTURE MODULE ============

// Enums for Lead Module
export const leadFieldTypeEnum = pgEnum('lead_field_type', ['TEXT', 'EMAIL', 'PHONE', 'CHECKBOX', 'SELECT', 'TEXTAREA', 'DATE', 'URL', 'NUMBER']);
export const leadSourceTypeEnum = pgEnum('lead_source_type', ['DIRECT', 'REFERRAL', 'CAMPAIGN', 'ORGANIC', 'PAID', 'IMPORT', 'BOOKING', 'GOOGLE_CONTACTS', 'FORM']);
export const leadStatusEnum = pgEnum('lead_status', ['NEW', 'CONTACTED', 'QUALIFIED', 'CONVERTED', 'LOST']);
export const channelTypeEnum = pgEnum('channel_type', ['EMAIL', 'SMS', 'WHATSAPP', 'VOICE', 'PUSH']);
export const triggerTypeEnum = pgEnum('trigger_type', ['ENTRY', 'STAGE_CHANGE', 'TIME_DELAY', 'MANUAL']);

// Lead Workflows (Funnels/Campaigns)
export const leadWorkflows = pgTable("lead_workflows", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  description: text("description"),
  isActive: boolean("is_active").notNull().default(true),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertLeadWorkflowSchema = createInsertSchema(leadWorkflows).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertLeadWorkflow = z.infer<typeof insertLeadWorkflowSchema>;
export type LeadWorkflow = typeof leadWorkflows.$inferSelect;

// Lead Workflow Stages (Kanban columns)
export const leadWorkflowStages = pgTable("lead_workflow_stages", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workflowId: varchar("workflow_id").notNull().references(() => leadWorkflows.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  color: text("color").default('#6366f1'),
  position: integer("position").notNull().default(0),
  isDefault: boolean("is_default").notNull().default(false),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertLeadWorkflowStageSchema = createInsertSchema(leadWorkflowStages).omit({ id: true, createdAt: true });
export type InsertLeadWorkflowStage = z.infer<typeof insertLeadWorkflowStageSchema>;
export type LeadWorkflowStage = typeof leadWorkflowStages.$inferSelect;

// Lead Forms
export const leadForms = pgTable("lead_forms", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  workflowId: varchar("workflow_id").references(() => leadWorkflows.id, { onDelete: 'set null' }),
  name: text("name").notNull(),
  description: text("description"),
  submitButtonText: text("submit_button_text").default('Enviar'),
  successMessage: text("success_message").default('Obrigado! Entraremos em contato em breve.'),
  isActive: boolean("is_active").notNull().default(true),
  isLeadGenerator: boolean("is_lead_generator").notNull().default(false),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertLeadFormSchema = createInsertSchema(leadForms).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertLeadForm = z.infer<typeof insertLeadFormSchema>;
export type LeadForm = typeof leadForms.$inferSelect;

// Lead Form Fields
export const leadFormFields = pgTable("lead_form_fields", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  formId: varchar("form_id").notNull().references(() => leadForms.id, { onDelete: 'cascade' }),
  type: leadFieldTypeEnum("type").notNull().default('TEXT'),
  label: text("label").notNull(),
  placeholder: text("placeholder"),
  required: boolean("required").notNull().default(false),
  options: text("options"), // JSON string for SELECT options
  position: integer("position").notNull().default(0),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertLeadFormFieldSchema = createInsertSchema(leadFormFields).omit({ id: true, createdAt: true });
export type InsertLeadFormField = z.infer<typeof insertLeadFormFieldSchema>;
export type LeadFormField = typeof leadFormFields.$inferSelect;

// Lead Landing Pages
export const leadLandingPages = pgTable("lead_landing_pages", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  formId: varchar("form_id").references(() => leadForms.id, { onDelete: 'set null' }),
  workflowId: varchar("workflow_id").references(() => leadWorkflows.id, { onDelete: 'set null' }),
  slug: varchar("slug", { length: 100 }).notNull().unique(),
  title: text("title").notNull(),
  subtitle: text("subtitle"),
  description: text("description"),
  logoUrl: text("logo_url"),
  primaryColor: text("primary_color").default('#6366f1'),
  backgroundColor: text("background_color").default('#ffffff'),
  showSocialProof: boolean("show_social_proof").notNull().default(false),
  socialProofText: text("social_proof_text"),
  footerText: text("footer_text").default('Powered by Dominus'),
  isActive: boolean("is_active").notNull().default(true),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertLeadLandingPageSchema = createInsertSchema(leadLandingPages).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertLeadLandingPage = z.infer<typeof insertLeadLandingPageSchema>;
export type LeadLandingPage = typeof leadLandingPages.$inferSelect;

// Leads
export const leads = pgTable("leads", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  workflowId: varchar("workflow_id").references(() => leadWorkflows.id, { onDelete: 'set null' }),
  stageId: varchar("stage_id").references(() => leadWorkflowStages.id, { onDelete: 'set null' }),
  landingPageId: varchar("landing_page_id").references(() => leadLandingPages.id, { onDelete: 'set null' }),
  
  // Contact info
  name: text("name"),
  email: text("email"),
  phone: text("phone"),
  customData: text("custom_data"), // JSON with form field responses
  
  // Referral system
  referralCode: varchar("referral_code", { length: 16 }).unique(),
  referredBy: varchar("referred_by", { length: 16 }),
  referralCount: integer("referral_count").notNull().default(0),
  
  // Queue/Waitlist
  queuePosition: integer("queue_position"),
  
  // Verification
  verified: boolean("verified").notNull().default(false),
  verificationToken: varchar("verification_token", { length: 64 }),
  verifiedAt: timestamp("verified_at"),
  
  // Status and source
  status: leadStatusEnum("status").notNull().default('NEW'),
  source: leadSourceTypeEnum("source").notNull().default('DIRECT'),
  
  // Notes/observations
  notes: text("notes"),
  
  // Conversion
  convertedToClientId: varchar("converted_to_client_id").references(() => clients.id, { onDelete: 'set null' }),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertLeadSchema = createInsertSchema(leads).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertLead = z.infer<typeof insertLeadSchema>;
export type Lead = typeof leads.$inferSelect;

// Lead Tags (for categorization like Arquitetos, Engenheiros, etc.)
export const leadTags = pgTable("lead_tags", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  color: varchar("color", { length: 7 }).default('#6b7280'),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertLeadTagSchema = createInsertSchema(leadTags).omit({ id: true, createdAt: true });
export type InsertLeadTag = z.infer<typeof insertLeadTagSchema>;
export type LeadTag = typeof leadTags.$inferSelect;

// Lead-Tag assignments (many-to-many relationship)
export const leadTagAssignments = pgTable("lead_tag_assignments", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  leadId: varchar("lead_id").notNull().references(() => leads.id, { onDelete: 'cascade' }),
  tagId: varchar("tag_id").notNull().references(() => leadTags.id, { onDelete: 'cascade' }),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertLeadTagAssignmentSchema = createInsertSchema(leadTagAssignments).omit({ id: true, createdAt: true });
export type InsertLeadTagAssignment = z.infer<typeof insertLeadTagAssignmentSchema>;
export type LeadTagAssignment = typeof leadTagAssignments.$inferSelect;

// Lead Form Submissions (records each form submission)
export const leadFormSubmissions = pgTable("lead_form_submissions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  leadId: varchar("lead_id").notNull().references(() => leads.id, { onDelete: 'cascade' }),
  formId: varchar("form_id").notNull().references(() => leadForms.id, { onDelete: 'cascade' }),
  data: text("data").notNull(), // JSON with field responses
  ipAddress: text("ip_address"),
  userAgent: text("user_agent"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertLeadFormSubmissionSchema = createInsertSchema(leadFormSubmissions).omit({ id: true, createdAt: true });
export type InsertLeadFormSubmission = z.infer<typeof insertLeadFormSubmissionSchema>;
export type LeadFormSubmission = typeof leadFormSubmissions.$inferSelect;

// Lead Stage History (audit trail for stage changes)
export const leadStageHistory = pgTable("lead_stage_history", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  leadId: varchar("lead_id").notNull().references(() => leads.id, { onDelete: 'cascade' }),
  fromStageId: varchar("from_stage_id").references(() => leadWorkflowStages.id, { onDelete: 'set null' }),
  toStageId: varchar("to_stage_id").notNull().references(() => leadWorkflowStages.id, { onDelete: 'cascade' }),
  changedBy: varchar("changed_by").references(() => users.id, { onDelete: 'set null' }),
  notes: text("notes"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertLeadStageHistorySchema = createInsertSchema(leadStageHistory).omit({ id: true, createdAt: true });
export type InsertLeadStageHistory = z.infer<typeof insertLeadStageHistorySchema>;
export type LeadStageHistory = typeof leadStageHistory.$inferSelect;

// Lead Message Templates
export const leadMessageTemplates = pgTable("lead_message_templates", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  channel: channelTypeEnum("channel").notNull(),
  subject: text("subject"), // For email
  content: text("content").notNull(),
  variables: text("variables"), // JSON array of available variables
  isActive: boolean("is_active").notNull().default(true),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertLeadMessageTemplateSchema = createInsertSchema(leadMessageTemplates).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertLeadMessageTemplate = z.infer<typeof insertLeadMessageTemplateSchema>;
export type LeadMessageTemplate = typeof leadMessageTemplates.$inferSelect;

// Lead Workflow Automations (rules for automatic actions)
export const leadAutomations = pgTable("lead_automations", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workflowId: varchar("workflow_id").notNull().references(() => leadWorkflows.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  trigger: triggerTypeEnum("trigger").notNull(),
  triggerStageId: varchar("trigger_stage_id").references(() => leadWorkflowStages.id, { onDelete: 'set null' }),
  delayMinutes: integer("delay_minutes").default(0),
  templateId: varchar("template_id").references(() => leadMessageTemplates.id, { onDelete: 'set null' }),
  isActive: boolean("is_active").notNull().default(true),
  position: integer("position").notNull().default(0),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertLeadAutomationSchema = createInsertSchema(leadAutomations).omit({ id: true, createdAt: true });
export type InsertLeadAutomation = z.infer<typeof insertLeadAutomationSchema>;
export type LeadAutomation = typeof leadAutomations.$inferSelect;

// Lead Scheduled Messages (queue for sending)
export const leadScheduledMessages = pgTable("lead_scheduled_messages", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  leadId: varchar("lead_id").notNull().references(() => leads.id, { onDelete: 'cascade' }),
  automationId: varchar("automation_id").references(() => leadAutomations.id, { onDelete: 'set null' }),
  templateId: varchar("template_id").references(() => leadMessageTemplates.id, { onDelete: 'set null' }),
  channel: channelTypeEnum("channel").notNull(),
  subject: text("subject"),
  content: text("content").notNull(),
  scheduledFor: timestamp("scheduled_for").notNull(),
  sentAt: timestamp("sent_at"),
  status: text("status").notNull().default('PENDING'), // PENDING, SENT, FAILED, CANCELLED
  errorMessage: text("error_message"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertLeadScheduledMessageSchema = createInsertSchema(leadScheduledMessages).omit({ id: true, createdAt: true });
export type InsertLeadScheduledMessage = z.infer<typeof insertLeadScheduledMessageSchema>;
export type LeadScheduledMessage = typeof leadScheduledMessages.$inferSelect;

// ============ CUSTOM FIELDS ============

// Entity types that can have custom fields
export const customFieldEntityEnum = pgEnum('custom_field_entity', ['CLIENT', 'PROJECT', 'PROPOSAL', 'INVOICE', 'LEAD', 'TASK']);

// Field types
export const customFieldTypeEnum = pgEnum('custom_field_type', ['TEXT', 'TEXTAREA', 'NUMBER', 'CURRENCY', 'DATE', 'CHECKBOX', 'SELECT', 'MULTISELECT', 'URL', 'EMAIL', 'PHONE']);

// Custom Field Definitions
export const customFieldDefinitions = pgTable("custom_field_definitions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  entityType: customFieldEntityEnum("entity_type").notNull(),
  fieldKey: varchar("field_key", { length: 100 }).notNull(),
  label: text("label").notNull(),
  fieldType: customFieldTypeEnum("field_type").notNull(),
  isRequired: boolean("is_required").notNull().default(false),
  placeholder: text("placeholder"),
  helpText: text("help_text"),
  defaultValue: text("default_value"),
  options: text("options"), // JSON array for SELECT/MULTISELECT options
  settings: text("settings"), // JSON for additional settings (min, max, pattern, etc.)
  position: integer("position").notNull().default(0),
  isActive: boolean("is_active").notNull().default(true),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertCustomFieldDefinitionSchema = createInsertSchema(customFieldDefinitions).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertCustomFieldDefinition = z.infer<typeof insertCustomFieldDefinitionSchema>;
export type CustomFieldDefinition = typeof customFieldDefinitions.$inferSelect;

// Custom Field Values
export const customFieldValues = pgTable("custom_field_values", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  definitionId: varchar("definition_id").notNull().references(() => customFieldDefinitions.id, { onDelete: 'cascade' }),
  entityType: customFieldEntityEnum("entity_type").notNull(),
  entityId: varchar("entity_id").notNull(),
  valueText: text("value_text"),
  valueNumber: decimal("value_number", { precision: 15, scale: 4 }),
  valueDate: timestamp("value_date"),
  valueBoolean: boolean("value_boolean"),
  valueJson: text("value_json"), // For MULTISELECT and complex data
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertCustomFieldValueSchema = createInsertSchema(customFieldValues).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertCustomFieldValue = z.infer<typeof insertCustomFieldValueSchema>;
export type CustomFieldValue = typeof customFieldValues.$inferSelect;

// ============ SCHEDULERS (BOOKING SYSTEM) ============

export const schedulerBookingStatusEnum = pgEnum('scheduler_booking_status', ['PENDING', 'CONFIRMED', 'CANCELLED', 'COMPLETED', 'NO_SHOW']);

// Scheduler Types (Meeting types like "Quick Call 15min", "Consultation 30min")
export const schedulerTypes = pgTable("scheduler_types", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  slug: varchar("slug", { length: 120 }).notNull(),
  description: text("description"),
  color: varchar("color", { length: 32 }).default("#2563eb"),
  durationMinutes: integer("duration_minutes").notNull(),
  bufferBeforeMinutes: integer("buffer_before_minutes").default(0),
  bufferAfterMinutes: integer("buffer_after_minutes").default(0),
  isActive: boolean("is_active").default(true),
  createMeetLink: boolean("create_meet_link").default(true),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertSchedulerTypeSchema = createInsertSchema(schedulerTypes).omit({ id: true, createdAt: true });
export type InsertSchedulerType = z.infer<typeof insertSchedulerTypeSchema>;
export type SchedulerType = typeof schedulerTypes.$inferSelect;

// Scheduler Availability (Weekly recurring availability windows)
export const schedulerAvailability = pgTable("scheduler_availability", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  schedulerTypeId: varchar("scheduler_type_id").notNull().references(() => schedulerTypes.id, { onDelete: 'cascade' }),
  dayOfWeek: integer("day_of_week").notNull(), // 0 = Sunday, 1 = Monday, ... 6 = Saturday
  startTime: text("start_time").notNull(), // "09:00" format
  endTime: text("end_time").notNull(), // "18:00" format
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertSchedulerAvailabilitySchema = createInsertSchema(schedulerAvailability).omit({ id: true, createdAt: true });
export type InsertSchedulerAvailability = z.infer<typeof insertSchedulerAvailabilitySchema>;
export type SchedulerAvailability = typeof schedulerAvailability.$inferSelect;

// Scheduler Availability Exceptions (specific dates blocked or with custom hours)
export const schedulerExceptions = pgTable("scheduler_exceptions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  schedulerTypeId: varchar("scheduler_type_id").notNull().references(() => schedulerTypes.id, { onDelete: 'cascade' }),
  date: date("date").notNull(),
  isBlocked: boolean("is_blocked").default(true),
  startTime: text("start_time"), // custom hours if not blocked
  endTime: text("end_time"),
  reason: text("reason"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertSchedulerExceptionSchema = createInsertSchema(schedulerExceptions).omit({ id: true, createdAt: true });
export type InsertSchedulerException = z.infer<typeof insertSchedulerExceptionSchema>;
export type SchedulerException = typeof schedulerExceptions.$inferSelect;

// Scheduler Bookings (appointments booked by clients)
export const schedulerBookings = pgTable("scheduler_bookings", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  schedulerTypeId: varchar("scheduler_type_id").notNull().references(() => schedulerTypes.id, { onDelete: 'cascade' }),
  clientName: text("client_name").notNull(),
  clientEmail: text("client_email").notNull(),
  clientPhone: text("client_phone"),
  notes: text("notes"),
  startTime: timestamp("start_time").notNull(),
  endTime: timestamp("end_time").notNull(),
  status: schedulerBookingStatusEnum("status").default("PENDING"),
  calendarEventId: text("calendar_event_id"),
  meetingLink: text("meeting_link"),
  clientId: varchar("client_id").references(() => clients.id, { onDelete: 'set null' }),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertSchedulerBookingSchema = createInsertSchema(schedulerBookings).omit({ id: true, createdAt: true });
export type InsertSchedulerBooking = z.infer<typeof insertSchedulerBookingSchema>;
export type SchedulerBooking = typeof schedulerBookings.$inferSelect;

// ============ ROLE PERMISSIONS CONFIG ============

export type PermissionModule = 'CLIENTS' | 'PROJECTS' | 'TASKS' | 'PROPOSALS' | 'INVOICES' | 'LEADS' | 'PRODUCTS' | 'TRANSACTIONS' | 'SETTINGS' | 'TEAM';
export type PermissionAction = 'VIEW' | 'CREATE' | 'EDIT' | 'DELETE';
export type MemberRole = 'ADMIN' | 'GERENTE' | 'VENDEDOR' | 'FINANCEIRO' | 'MEMBRO';

export const ALL_MODULES: PermissionModule[] = ['CLIENTS', 'PROJECTS', 'TASKS', 'PROPOSALS', 'INVOICES', 'LEADS', 'PRODUCTS', 'TRANSACTIONS', 'SETTINGS', 'TEAM'];
export const ALL_ACTIONS: PermissionAction[] = ['VIEW', 'CREATE', 'EDIT', 'DELETE'];

export const DEFAULT_ROLE_PERMISSIONS: Record<MemberRole, Record<PermissionModule, PermissionAction[]>> = {
  ADMIN: {
    CLIENTS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    PROJECTS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    TASKS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    PROPOSALS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    INVOICES: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    LEADS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    PRODUCTS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    TRANSACTIONS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    SETTINGS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    TEAM: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
  },
  GERENTE: {
    CLIENTS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    PROJECTS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    TASKS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    PROPOSALS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    INVOICES: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    LEADS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    PRODUCTS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    TRANSACTIONS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    SETTINGS: ['VIEW'],
    TEAM: ['VIEW'],
  },
  VENDEDOR: {
    CLIENTS: ['VIEW', 'CREATE', 'EDIT'],
    PROJECTS: ['VIEW'],
    TASKS: ['VIEW', 'CREATE', 'EDIT'],
    PROPOSALS: ['VIEW', 'CREATE', 'EDIT'],
    INVOICES: ['VIEW'],
    LEADS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    PRODUCTS: ['VIEW'],
    TRANSACTIONS: [],
    SETTINGS: [],
    TEAM: [],
  },
  FINANCEIRO: {
    CLIENTS: ['VIEW'],
    PROJECTS: ['VIEW'],
    TASKS: [],
    PROPOSALS: ['VIEW'],
    INVOICES: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    LEADS: [],
    PRODUCTS: ['VIEW'],
    TRANSACTIONS: ['VIEW', 'CREATE', 'EDIT', 'DELETE'],
    SETTINGS: [],
    TEAM: [],
  },
  MEMBRO: {
    CLIENTS: ['VIEW'],
    PROJECTS: ['VIEW'],
    TASKS: ['VIEW', 'CREATE', 'EDIT'],
    PROPOSALS: ['VIEW'],
    INVOICES: ['VIEW'],
    LEADS: ['VIEW'],
    PRODUCTS: ['VIEW'],
    TRANSACTIONS: [],
    SETTINGS: [],
    TEAM: [],
  },
};

export const MODULE_LABELS: Record<PermissionModule, string> = {
  CLIENTS: 'Clientes',
  PROJECTS: 'Projetos',
  TASKS: 'Tarefas',
  PROPOSALS: 'Propostas',
  INVOICES: 'Faturas',
  LEADS: 'Leads',
  PRODUCTS: 'Produtos',
  TRANSACTIONS: 'Financeiro',
  SETTINGS: 'Configurações',
  TEAM: 'Equipe',
};

export const ACTION_LABELS: Record<PermissionAction, string> = {
  VIEW: 'Visualizar',
  CREATE: 'Criar',
  EDIT: 'Editar',
  DELETE: 'Excluir',
};

export const ROLE_LABELS: Record<MemberRole, string> = {
  ADMIN: 'Administrador',
  GERENTE: 'Gerente',
  VENDEDOR: 'Vendedor',
  FINANCEIRO: 'Financeiro',
  MEMBRO: 'Membro',
};

// ============ SITE BUILDER MODULE ============

// Enums for Site Builder
export const siteStatusEnum = pgEnum('site_status', ['DRAFT', 'PUBLISHED', 'ARCHIVED']);
export const pageStatusEnum = pgEnum('page_status', ['DRAFT', 'PUBLISHED', 'ARCHIVED']);
export const blogPostStatusEnum = pgEnum('blog_post_status', ['DRAFT', 'PUBLISHED', 'SCHEDULED', 'ARCHIVED']);
export const blockCategoryEnum = pgEnum('block_category', ['HERO', 'FEATURES', 'TESTIMONIALS', 'PRICING', 'CTA', 'FAQ', 'FOOTER', 'HEADER', 'CONTENT', 'GALLERY', 'FORM', 'BLOG', 'UTILITY']);

// Site Projects (main site/project container)
export const siteProjects = pgTable("site_projects", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  slug: varchar("slug", { length: 100 }).notNull(),
  description: text("description"),
  status: siteStatusEnum("status").notNull().default('DRAFT'),
  
  // Theme settings
  primaryColor: varchar("primary_color", { length: 7 }).default('#3B82F6'),
  secondaryColor: varchar("secondary_color", { length: 7 }).default('#10B981'),
  fontFamily: text("font_family").default('Inter'),
  
  // SEO defaults
  seoTitle: text("seo_title"),
  seoDescription: text("seo_description"),
  seoImage: text("seo_image"),
  
  // Domain settings
  customDomain: text("custom_domain"),
  
  // Branding
  logoUrl: text("logo_url"),
  faviconUrl: text("favicon_url"),
  
  // Custom scripts
  headScripts: text("head_scripts"),
  bodyStartScripts: text("body_start_scripts"),
  bodyEndScripts: text("body_end_scripts"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertSiteProjectSchema = createInsertSchema(siteProjects).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertSiteProject = z.infer<typeof insertSiteProjectSchema>;
export type SiteProject = typeof siteProjects.$inferSelect;

// Site Pages (individual pages within a site)
export const sitePages = pgTable("site_pages", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  siteId: varchar("site_id").notNull().references(() => siteProjects.id, { onDelete: 'cascade' }),
  parentPageId: varchar("parent_page_id").references((): any => sitePages.id, { onDelete: 'set null' }),
  
  title: text("title").notNull(),
  slug: varchar("slug", { length: 200 }).notNull(),
  description: text("description"),
  status: pageStatusEnum("status").notNull().default('DRAFT'),
  
  // Page settings
  isHomePage: boolean("is_home_page").notNull().default(false),
  showInNavigation: boolean("show_in_navigation").notNull().default(true),
  navigationOrder: integer("navigation_order").notNull().default(0),
  
  // SEO override
  seoTitle: text("seo_title"),
  seoDescription: text("seo_description"),
  seoImage: text("seo_image"),
  
  // Current content (JSON array of blocks)
  content: text("content").default('[]'),
  
  // Versioning
  currentVersion: integer("current_version").notNull().default(1),
  publishedVersion: integer("published_version"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertSitePageSchema = createInsertSchema(sitePages).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertSitePage = z.infer<typeof insertSitePageSchema>;
export type SitePage = typeof sitePages.$inferSelect;

// Site Page Versions (history of page content)
export const sitePageVersions = pgTable("site_page_versions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  pageId: varchar("page_id").notNull().references(() => sitePages.id, { onDelete: 'cascade' }),
  
  versionNumber: integer("version_number").notNull(),
  content: text("content").notNull(), // JSON array of blocks
  
  createdBy: varchar("created_by").references(() => users.id, { onDelete: 'set null' }),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  publishedAt: timestamp("published_at"),
});

export const insertSitePageVersionSchema = createInsertSchema(sitePageVersions).omit({ id: true, createdAt: true });
export type InsertSitePageVersion = z.infer<typeof insertSitePageVersionSchema>;
export type SitePageVersion = typeof sitePageVersions.$inferSelect;

// Blog Categories
export const blogCategories = pgTable("blog_categories", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  siteId: varchar("site_id").notNull().references(() => siteProjects.id, { onDelete: 'cascade' }),
  name: text("name").notNull(),
  slug: varchar("slug", { length: 100 }).notNull(),
  description: text("description"),
  color: varchar("color", { length: 7 }).default('#3B82F6'),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertBlogCategorySchema = createInsertSchema(blogCategories).omit({ id: true, createdAt: true });
export type InsertBlogCategory = z.infer<typeof insertBlogCategorySchema>;
export type BlogCategory = typeof blogCategories.$inferSelect;

// Blog Posts
export const blogPosts = pgTable("blog_posts", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  siteId: varchar("site_id").notNull().references(() => siteProjects.id, { onDelete: 'cascade' }),
  authorId: varchar("author_id").references(() => users.id, { onDelete: 'set null' }),
  categoryId: varchar("category_id").references(() => blogCategories.id, { onDelete: 'set null' }),
  
  title: text("title").notNull(),
  slug: varchar("slug", { length: 300 }).notNull(),
  excerpt: text("excerpt"),
  coverImage: text("cover_image"),
  
  status: blogPostStatusEnum("status").notNull().default('DRAFT'),
  
  // Content (JSON array of blocks, same format as pages)
  content: text("content").default('[]'),
  
  // SEO
  seoTitle: text("seo_title"),
  seoDescription: text("seo_description"),
  
  // Scheduling
  scheduledAt: timestamp("scheduled_at"),
  publishedAt: timestamp("published_at"),
  
  // Versioning
  currentVersion: integer("current_version").notNull().default(1),
  publishedVersion: integer("published_version"),
  
  // Metrics
  viewCount: integer("view_count").notNull().default(0),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertBlogPostSchema = createInsertSchema(blogPosts).omit({ id: true, createdAt: true, updatedAt: true, viewCount: true });
export type InsertBlogPost = z.infer<typeof insertBlogPostSchema>;
export type BlogPost = typeof blogPosts.$inferSelect;

// Blog Post Versions
export const blogPostVersions = pgTable("blog_post_versions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  postId: varchar("post_id").notNull().references(() => blogPosts.id, { onDelete: 'cascade' }),
  
  versionNumber: integer("version_number").notNull(),
  content: text("content").notNull(), // JSON array of blocks
  
  createdBy: varchar("created_by").references(() => users.id, { onDelete: 'set null' }),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  publishedAt: timestamp("published_at"),
});

export const insertBlogPostVersionSchema = createInsertSchema(blogPostVersions).omit({ id: true, createdAt: true });
export type InsertBlogPostVersion = z.infer<typeof insertBlogPostVersionSchema>;
export type BlogPostVersion = typeof blogPostVersions.$inferSelect;

// Site Visit Events (analytics)
export const siteVisitEvents = pgTable("site_visit_events", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  siteId: varchar("site_id").notNull().references(() => siteProjects.id, { onDelete: 'cascade' }),
  pageId: varchar("page_id").references(() => sitePages.id, { onDelete: 'set null' }),
  blogPostId: varchar("blog_post_id").references(() => blogPosts.id, { onDelete: 'set null' }),
  
  // Visitor info
  visitorId: varchar("visitor_id", { length: 64 }), // Anonymous hash
  sessionId: varchar("session_id", { length: 64 }),
  
  // Referral data
  referrer: text("referrer"),
  utmSource: text("utm_source"),
  utmMedium: text("utm_medium"),
  utmCampaign: text("utm_campaign"),
  
  // Device info
  userAgent: text("user_agent"),
  deviceType: varchar("device_type", { length: 20 }), // desktop, mobile, tablet
  country: varchar("country", { length: 2 }),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertSiteVisitEventSchema = createInsertSchema(siteVisitEvents).omit({ id: true, createdAt: true });
export type InsertSiteVisitEvent = z.infer<typeof insertSiteVisitEventSchema>;
export type SiteVisitEvent = typeof siteVisitEvents.$inferSelect;

// Site Visit Daily Stats (aggregated for performance)
export const siteVisitDailyStats = pgTable("site_visit_daily_stats", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  siteId: varchar("site_id").notNull().references(() => siteProjects.id, { onDelete: 'cascade' }),
  pageId: varchar("page_id").references(() => sitePages.id, { onDelete: 'set null' }),
  blogPostId: varchar("blog_post_id").references(() => blogPosts.id, { onDelete: 'set null' }),
  
  date: date("date").notNull(),
  
  // Metrics
  pageViews: integer("page_views").notNull().default(0),
  uniqueVisitors: integer("unique_visitors").notNull().default(0),
  
  // Source breakdown (JSON)
  sourceBreakdown: text("source_breakdown"), // { "google": 10, "direct": 5, ... }
  deviceBreakdown: text("device_breakdown"), // { "desktop": 10, "mobile": 5, ... }
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertSiteVisitDailyStatSchema = createInsertSchema(siteVisitDailyStats).omit({ id: true, createdAt: true });
export type InsertSiteVisitDailyStat = z.infer<typeof insertSiteVisitDailyStatSchema>;
export type SiteVisitDailyStat = typeof siteVisitDailyStats.$inferSelect;

// Block definition type for content JSON
export interface BlockContent {
  id: string;
  type: string; // Block type key (e.g., 'hero-simple', 'features-grid')
  props: Record<string, any>; // Block-specific properties
  children?: BlockContent[];
}

export const blockContentSchema: z.ZodType<BlockContent> = z.object({
  id: z.string(),
  type: z.string(),
  props: z.record(z.any()),
  children: z.array(z.lazy(() => blockContentSchema)).optional(),
});

// Block categories with labels
export const BLOCK_CATEGORY_LABELS: Record<string, string> = {
  HERO: 'Hero',
  FEATURES: 'Recursos',
  TESTIMONIALS: 'Depoimentos',
  PRICING: 'Preços',
  CTA: 'Call to Action',
  FAQ: 'FAQ',
  FOOTER: 'Rodapé',
  HEADER: 'Cabeçalho',
  CONTENT: 'Conteúdo',
  GALLERY: 'Galeria',
  FORM: 'Formulário',
  BLOG: 'Blog',
  UTILITY: 'Utilitários',
};

// ================================
// COLLABORATION & COMMUNICATION
// ================================

// Conversation types enum
export const conversationTypeEnum = pgEnum('conversation_type', ['DIRECT', 'GROUP', 'PROJECT', 'CLIENT']);

// Conversations (chat threads)
export const conversations = pgTable("conversations", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  type: conversationTypeEnum("type").notNull().default('DIRECT'),
  name: text("name"), // For group chats
  projectId: varchar("project_id").references(() => projects.id, { onDelete: 'set null' }),
  clientId: varchar("client_id").references(() => clients.id, { onDelete: 'set null' }),
  createdBy: varchar("created_by").notNull().references(() => users.id),
  lastMessageAt: timestamp("last_message_at"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertConversationSchema = createInsertSchema(conversations).omit({ id: true, createdAt: true });
export type InsertConversation = z.infer<typeof insertConversationSchema>;
export type Conversation = typeof conversations.$inferSelect;

// Conversation Participants
export const conversationParticipants = pgTable("conversation_participants", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  conversationId: varchar("conversation_id").notNull().references(() => conversations.id, { onDelete: 'cascade' }),
  userId: varchar("user_id").notNull().references(() => users.id, { onDelete: 'cascade' }),
  lastReadAt: timestamp("last_read_at"),
  isMuted: boolean("is_muted").notNull().default(false),
  joinedAt: timestamp("joined_at").defaultNow().notNull(),
});

export const insertConversationParticipantSchema = createInsertSchema(conversationParticipants).omit({ id: true, joinedAt: true });
export type InsertConversationParticipant = z.infer<typeof insertConversationParticipantSchema>;
export type ConversationParticipant = typeof conversationParticipants.$inferSelect;

// Messages (chat messages)
export const messages = pgTable("messages", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  conversationId: varchar("conversation_id").notNull().references(() => conversations.id, { onDelete: 'cascade' }),
  senderId: varchar("sender_id").notNull().references(() => users.id),
  content: text("content").notNull(),
  replyToId: varchar("reply_to_id"), // For replies
  isEdited: boolean("is_edited").notNull().default(false),
  editedAt: timestamp("edited_at"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertMessageSchema = createInsertSchema(messages).omit({ id: true, createdAt: true, isEdited: true, editedAt: true });
export type InsertMessage = z.infer<typeof insertMessageSchema>;
export type Message = typeof messages.$inferSelect;

// Message attachments
export const messageAttachments = pgTable("message_attachments", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  messageId: varchar("message_id").notNull().references(() => messages.id, { onDelete: 'cascade' }),
  fileName: text("file_name").notNull(),
  fileUrl: text("file_url").notNull(),
  fileSize: integer("file_size"),
  mimeType: text("mime_type"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertMessageAttachmentSchema = createInsertSchema(messageAttachments).omit({ id: true, createdAt: true });
export type InsertMessageAttachment = z.infer<typeof insertMessageAttachmentSchema>;
export type MessageAttachment = typeof messageAttachments.$inferSelect;

// Comment entity type enum
export const commentEntityTypeEnum = pgEnum('comment_entity_type', ['TASK', 'PROJECT', 'PROPOSAL', 'CONTRACT', 'INVOICE', 'LEAD']);

// Comments (generic comments for any entity)
export const comments = pgTable("comments", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  entityType: commentEntityTypeEnum("entity_type").notNull(),
  entityId: varchar("entity_id").notNull(),
  authorId: varchar("author_id").notNull().references(() => users.id),
  content: text("content").notNull(),
  parentId: varchar("parent_id"), // For nested replies
  isInternal: boolean("is_internal").notNull().default(false), // Internal-only comments
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertCommentSchema = createInsertSchema(comments).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertComment = z.infer<typeof insertCommentSchema>;
export type Comment = typeof comments.$inferSelect;

// Files (workspace file storage)
export const files = pgTable("files", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  uploadedBy: varchar("uploaded_by").notNull().references(() => users.id),
  
  // File info
  fileName: text("file_name").notNull(),
  originalName: text("original_name").notNull(),
  fileUrl: text("file_url").notNull(),
  fileSize: integer("file_size").notNull(),
  mimeType: text("mime_type"),
  
  // Organization
  folder: text("folder"), // Virtual folder path
  
  // Associations (optional)
  projectId: varchar("project_id").references(() => projects.id, { onDelete: 'set null' }),
  clientId: varchar("client_id").references(() => clients.id, { onDelete: 'set null' }),
  taskId: varchar("task_id").references(() => tasks.id, { onDelete: 'set null' }),
  
  // Visibility
  isPublic: boolean("is_public").notNull().default(false),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertFileSchema = createInsertSchema(files).omit({ id: true, createdAt: true });
export type InsertFile = z.infer<typeof insertFileSchema>;
export type File = typeof files.$inferSelect;

// Document type enum for client documents
export const documentTypeEnum = pgEnum('document_type', ['CONTRACT', 'LICENSE', 'CERTIFICATE', 'PERMIT', 'ID', 'OTHER']);
export const documentStatusEnum = pgEnum('document_status', ['VALID', 'EXPIRING_SOON', 'EXPIRED', 'PENDING']);

// Client Documents (with expiration tracking)
export const clientDocuments = pgTable("client_documents", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  clientId: varchar("client_id").notNull().references(() => clients.id, { onDelete: 'cascade' }),
  uploadedBy: varchar("uploaded_by").notNull().references(() => users.id),
  
  // Document info
  name: text("name").notNull(),
  description: text("description"),
  documentType: documentTypeEnum("document_type").notNull().default('OTHER'),
  documentNumber: text("document_number"), // Document identifier
  
  // File
  fileName: text("file_name").notNull(),
  fileUrl: text("file_url").notNull(),
  fileSize: integer("file_size"),
  mimeType: text("mime_type"),
  
  // Validity dates
  issueDate: date("issue_date"),
  expirationDate: date("expiration_date"),
  status: documentStatusEnum("status").notNull().default('VALID'),
  
  // Alerts
  alertDaysBefore: integer("alert_days_before").default(30), // Days before expiration to alert
  lastAlertSentAt: timestamp("last_alert_sent_at"),
  
  // Metadata
  notes: text("notes"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertClientDocumentSchema = createInsertSchema(clientDocuments).omit({ id: true, createdAt: true, updatedAt: true, status: true });
export type InsertClientDocument = z.infer<typeof insertClientDocumentSchema>;
export type ClientDocument = typeof clientDocuments.$inferSelect;

// Notifications
export const notificationTypeEnum = pgEnum('notification_type', ['MESSAGE', 'COMMENT', 'TASK_ASSIGNED', 'TASK_COMPLETED', 'DOCUMENT_EXPIRING', 'MENTION', 'SYSTEM']);

export const notifications = pgTable("notifications", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  userId: varchar("user_id").notNull().references(() => users.id, { onDelete: 'cascade' }),
  
  type: notificationTypeEnum("type").notNull(),
  title: text("title").notNull(),
  message: text("message").notNull(),
  
  // Reference to related entity
  entityType: text("entity_type"),
  entityId: varchar("entity_id"),
  
  // Read status
  isRead: boolean("is_read").notNull().default(false),
  readAt: timestamp("read_at"),
  
  // Metadata (JSON for extra context)
  metadata: text("metadata"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertNotificationSchema = createInsertSchema(notifications).omit({ id: true, createdAt: true, isRead: true, readAt: true });
export type InsertNotification = z.infer<typeof insertNotificationSchema>;
export type Notification = typeof notifications.$inferSelect;

// ==========================================
// AUTOMATION & SUPER WORK AI
// ==========================================

// Workflow Enums
export const workflowStatusEnum = pgEnum('workflow_status', ['ACTIVE', 'PAUSED', 'DRAFT']);
export const workflowTriggerTypeEnum = pgEnum('workflow_trigger_type', [
  'CLIENT_CREATED',
  'CLIENT_UPDATED',
  'PROJECT_CREATED',
  'PROJECT_STATUS_CHANGED',
  'PROJECT_COMPLETED',
  'TASK_CREATED',
  'TASK_COMPLETED',
  'TASK_ASSIGNED',
  'TASK_DUE_SOON',
  'TASK_OVERDUE',
  'PROPOSAL_CREATED',
  'PROPOSAL_SENT',
  'PROPOSAL_ACCEPTED',
  'PROPOSAL_REJECTED',
  'INVOICE_CREATED',
  'INVOICE_SENT',
  'INVOICE_PAID',
  'INVOICE_OVERDUE',
  'CONTRACT_CREATED',
  'CONTRACT_SIGNED',
  'LEAD_CREATED',
  'LEAD_STATUS_CHANGED',
  'TRANSACTION_CREATED',
  'TRANSACTION_DUE_SOON',
  'DOCUMENT_EXPIRING',
  'SCHEDULED', // Time-based trigger
]);

export const workflowActionTypeEnum = pgEnum('workflow_action_type', [
  'SEND_NOTIFICATION',
  'CREATE_TASK',
  'UPDATE_STATUS',
  'SEND_EMAIL',
  'SEND_SMS',
  'SEND_WHATSAPP',
  'CREATE_COMMENT',
  'ASSIGN_USER',
  'GENERATE_AI_CONTENT',
  'WAIT_DELAY',
  'CREATE_TRANSACTION',
  'WEBHOOK',
]);

export const workflowExecutionStatusEnum = pgEnum('workflow_execution_status', ['PENDING', 'RUNNING', 'COMPLETED', 'FAILED', 'CANCELLED']);

// Workflows - automation rules
export const workflows = pgTable("workflows", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  createdBy: varchar("created_by").notNull().references(() => users.id),
  
  // Basic info
  name: text("name").notNull(),
  description: text("description"),
  status: workflowStatusEnum("status").notNull().default('DRAFT'),
  
  // Trigger configuration
  triggerType: workflowTriggerTypeEnum("trigger_type").notNull(),
  triggerConditions: text("trigger_conditions"), // JSON: filter conditions for trigger
  
  // For scheduled triggers
  scheduleExpression: text("schedule_expression"), // Cron expression or interval
  nextScheduledRun: timestamp("next_scheduled_run"),
  
  // Actions to execute (JSON array of action objects)
  actions: text("actions").notNull(), // JSON array of {type, config, order}
  
  // Stats
  executionCount: integer("execution_count").notNull().default(0),
  lastExecutedAt: timestamp("last_executed_at"),
  
  // Template info
  isTemplate: boolean("is_template").notNull().default(false),
  templateCategory: text("template_category"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertWorkflowSchema = createInsertSchema(workflows).omit({ id: true, createdAt: true, updatedAt: true, executionCount: true, lastExecutedAt: true });
export type InsertWorkflow = z.infer<typeof insertWorkflowSchema>;
export type Workflow = typeof workflows.$inferSelect;

// Workflow Executions - log of workflow runs
export const workflowExecutions = pgTable("workflow_executions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workflowId: varchar("workflow_id").notNull().references(() => workflows.id, { onDelete: 'cascade' }),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  
  // Trigger info
  triggerType: workflowTriggerTypeEnum("trigger_type").notNull(),
  triggerData: text("trigger_data"), // JSON: data that triggered the workflow
  
  // Execution status
  status: workflowExecutionStatusEnum("status").notNull().default('PENDING'),
  
  // Execution details
  stepsCompleted: integer("steps_completed").notNull().default(0),
  totalSteps: integer("total_steps").notNull().default(0),
  currentStep: text("current_step"),
  
  // Results
  result: text("result"), // JSON: results from each action
  errorMessage: text("error_message"),
  
  startedAt: timestamp("started_at"),
  completedAt: timestamp("completed_at"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertWorkflowExecutionSchema = createInsertSchema(workflowExecutions).omit({ id: true, createdAt: true, startedAt: true, completedAt: true });
export type InsertWorkflowExecution = z.infer<typeof insertWorkflowExecutionSchema>;
export type WorkflowExecution = typeof workflowExecutions.$inferSelect;

// AI Chat Sessions - Super Work AI conversations
export const aiChatSessions = pgTable("ai_chat_sessions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  userId: varchar("user_id").notNull().references(() => users.id, { onDelete: 'cascade' }),
  
  title: text("title").notNull().default('Nova conversa'),
  
  // Context for AI
  contextType: text("context_type"), // 'project', 'client', 'task', etc.
  contextId: varchar("context_id"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertAiChatSessionSchema = createInsertSchema(aiChatSessions).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertAiChatSession = z.infer<typeof insertAiChatSessionSchema>;
export type AiChatSession = typeof aiChatSessions.$inferSelect;

// AI Chat Messages
export const aiChatRoleEnum = pgEnum('ai_chat_role', ['user', 'assistant', 'system']);

export const aiChatMessages = pgTable("ai_chat_messages", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  sessionId: varchar("session_id").notNull().references(() => aiChatSessions.id, { onDelete: 'cascade' }),
  
  role: aiChatRoleEnum("role").notNull(),
  content: text("content").notNull(),
  
  // If AI executed an action
  actionExecuted: text("action_executed"), // JSON: {type, params, result}
  
  // Token usage (for tracking)
  tokensUsed: integer("tokens_used"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertAiChatMessageSchema = createInsertSchema(aiChatMessages).omit({ id: true, createdAt: true });
export type InsertAiChatMessage = z.infer<typeof insertAiChatMessageSchema>;
export type AiChatMessage = typeof aiChatMessages.$inferSelect;

// ==========================================
// HOST ADMIN PANEL
// ==========================================

// Workspace Subscriptions - tracks plan and billing for each workspace
export const workspaceSubscriptions = pgTable("workspace_subscriptions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }).unique(),
  
  plan: subscriptionPlanEnum("plan").notNull().default('TRIAL'),
  status: subscriptionStatusEnum("status").notNull().default('TRIAL'),
  
  // Limits
  maxUsers: integer("max_users").notNull().default(3),
  maxProjects: integer("max_projects").notNull().default(5),
  maxStorage: integer("max_storage").notNull().default(1024), // MB
  
  // Feature flags - what's included in the plan
  canUseSms: boolean("can_use_sms").notNull().default(false),
  canUseWhatsapp: boolean("can_use_whatsapp").notNull().default(false),
  canUseChatbot: boolean("can_use_chatbot").notNull().default(false),
  canUseAi: boolean("can_use_ai").notNull().default(false),
  
  // Quotas - monthly limits (0 = unlimited for Enterprise, null = feature disabled)
  smsQuota: integer("sms_quota").notNull().default(0),
  whatsappQuota: integer("whatsapp_quota").notNull().default(0),
  smsUsed: integer("sms_used").notNull().default(0),
  whatsappUsed: integer("whatsapp_used").notNull().default(0),
  quotaResetAt: timestamp("quota_reset_at"),
  
  // Trial info
  trialEndsAt: timestamp("trial_ends_at"),
  
  // Billing
  billingEmail: text("billing_email"),
  nextBillingDate: timestamp("next_billing_date"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertWorkspaceSubscriptionSchema = createInsertSchema(workspaceSubscriptions).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertWorkspaceSubscription = z.infer<typeof insertWorkspaceSubscriptionSchema>;
export type WorkspaceSubscription = typeof workspaceSubscriptions.$inferSelect;

// Workspace Usage Metrics - aggregated usage stats
export const workspaceUsageMetrics = pgTable("workspace_usage_metrics", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  
  // Date for this metric record
  date: date("date").notNull(),
  
  // Activity metrics
  uniqueLogins: integer("unique_logins").notNull().default(0),
  totalSessions: integer("total_sessions").notNull().default(0),
  apiCalls: integer("api_calls").notNull().default(0),
  
  // Entity counts
  clientsCount: integer("clients_count").notNull().default(0),
  projectsCount: integer("projects_count").notNull().default(0),
  tasksCount: integer("tasks_count").notNull().default(0),
  proposalsCount: integer("proposals_count").notNull().default(0),
  invoicesCount: integer("invoices_count").notNull().default(0),
  
  // Storage used in MB
  storageUsed: integer("storage_used").notNull().default(0),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertWorkspaceUsageMetricsSchema = createInsertSchema(workspaceUsageMetrics).omit({ id: true, createdAt: true });
export type InsertWorkspaceUsageMetrics = z.infer<typeof insertWorkspaceUsageMetricsSchema>;
export type WorkspaceUsageMetrics = typeof workspaceUsageMetrics.$inferSelect;

// Workspace Access Events - individual access logs
export const workspaceAccessEvents = pgTable("workspace_access_events", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  userId: varchar("user_id").notNull().references(() => users.id, { onDelete: 'cascade' }),
  
  eventType: text("event_type").notNull(), // 'login', 'api_call', 'page_view'
  path: text("path"),
  ipAddress: text("ip_address"),
  userAgent: text("user_agent"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertWorkspaceAccessEventSchema = createInsertSchema(workspaceAccessEvents).omit({ id: true, createdAt: true });
export type InsertWorkspaceAccessEvent = z.infer<typeof insertWorkspaceAccessEventSchema>;
export type WorkspaceAccessEvent = typeof workspaceAccessEvents.$inferSelect;

// Landing Page Leads - capture interested visitors from landing page
export const landingLeads = pgTable("landing_leads", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  name: text("name").notNull(),
  email: text("email").notNull(),
  company: text("company"),
  message: text("message"),
  source: text("source").default('landing'), // landing, blog, referral, etc
  status: text("status").notNull().default('NEW'), // NEW, CONTACTED, QUALIFIED, CONVERTED
  notes: text("notes"),
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertLandingLeadSchema = createInsertSchema(landingLeads).omit({ id: true, createdAt: true, updatedAt: true }).extend({
  source: z.string().optional().default('landing'),
  status: z.string().optional().default('NEW'),
  notes: z.string().nullable().optional(),
});
export type InsertLandingLead = z.infer<typeof insertLandingLeadSchema>;
export type LandingLead = typeof landingLeads.$inferSelect;

// ============ VISUAL PROPOSAL BUILDER ============

// Block types for visual proposals
export const proposalBlockTypeEnum = pgEnum('proposal_block_type', [
  'HERO',           // Cover with title, subtitle, client name, background image
  'ABOUT',          // About us section with description and achievements
  'PORTFOLIO',      // Grid of previous work/campaigns
  'TESTIMONIAL',    // Client testimonials/quotes
  'NEED',           // Client need/problem description
  'SERVICES_TABLE', // Pricing table with proposal items
  'INVESTMENT',     // Total investment summary
  'TERMS',          // Terms and conditions
  'SIGNATURE',      // Signature area
  'TEXT',           // Free text block
  'IMAGE',          // Single image block
  'TWO_COLUMNS',    // Two column layout
  'DIVIDER',        // Visual divider/separator
]);

// Proposal Visual Templates
export const proposalTemplates = pgTable("proposal_templates", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").references(() => workspaces.id, { onDelete: 'cascade' }), // null = global template
  name: text("name").notNull(),
  description: text("description"),
  
  // Template style configuration
  style: text("style").notNull().default('{}'), // JSON: colors, fonts, spacing
  thumbnail: text("thumbnail"), // Preview image URL
  
  // Template settings
  isDefault: boolean("is_default").notNull().default(false),
  isPublic: boolean("is_public").notNull().default(false), // Available to all workspaces
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertProposalTemplateSchema = createInsertSchema(proposalTemplates).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertProposalTemplate = z.infer<typeof insertProposalTemplateSchema>;
export type ProposalTemplate = typeof proposalTemplates.$inferSelect;

// Proposal Block Instances - blocks used in a specific proposal
export const proposalBlockInstances = pgTable("proposal_block_instances", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  proposalId: varchar("proposal_id").notNull().references(() => proposals.id, { onDelete: 'cascade' }),
  
  // Block configuration
  blockType: proposalBlockTypeEnum("block_type").notNull(),
  position: integer("position").notNull().default(0),
  
  // Block content (JSON) - varies by block type
  content: text("content").notNull().default('{}'),
  
  // Block-specific style overrides (JSON)
  style: text("style").notNull().default('{}'),
  
  // Visibility
  isVisible: boolean("is_visible").notNull().default(true),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertProposalBlockInstanceSchema = createInsertSchema(proposalBlockInstances).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertProposalBlockInstance = z.infer<typeof insertProposalBlockInstanceSchema>;
export type ProposalBlockInstance = typeof proposalBlockInstances.$inferSelect;

// Template Default Blocks - pre-configured blocks for templates
export const proposalTemplateBlocks = pgTable("proposal_template_blocks", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  templateId: varchar("template_id").notNull().references(() => proposalTemplates.id, { onDelete: 'cascade' }),
  
  // Block configuration
  blockType: proposalBlockTypeEnum("block_type").notNull(),
  position: integer("position").notNull().default(0),
  
  // Default content for this block in template
  defaultContent: text("default_content").notNull().default('{}'),
  
  // Default style for this block
  defaultStyle: text("default_style").notNull().default('{}'),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertProposalTemplateBlockSchema = createInsertSchema(proposalTemplateBlocks).omit({ id: true, createdAt: true });
export type InsertProposalTemplateBlock = z.infer<typeof insertProposalTemplateBlockSchema>;
export type ProposalTemplateBlock = typeof proposalTemplateBlocks.$inferSelect;

// Type for block type
export type ProposalBlockType = 'HERO' | 'ABOUT' | 'PORTFOLIO' | 'TESTIMONIAL' | 'NEED' | 'SERVICES_TABLE' | 'INVESTMENT' | 'TERMS' | 'SIGNATURE' | 'TEXT' | 'IMAGE' | 'TWO_COLUMNS' | 'DIVIDER';

// ============ EMAIL SYSTEM ============

// Email Template Trigger Types
export const emailTriggerTypeEnum = pgEnum('email_trigger_type', [
  'WELCOME',
  'PASSWORD_RESET',
  'PROPOSAL_SENT',
  'PROPOSAL_VIEWED',
  'PROPOSAL_ACCEPTED',
  'PROPOSAL_REJECTED',
  'CONTRACT_SENT',
  'CONTRACT_SIGNED',
  'INVOICE_SENT',
  'INVOICE_PAID',
  'INVOICE_OVERDUE',
  'TASK_ASSIGNED',
  'TASK_COMPLETED',
  'PROJECT_CREATED',
  'PROJECT_COMPLETED',
  'DOCUMENT_EXPIRING',
  'BOOKING_CONFIRMED',
  'BOOKING_REMINDER',
  'CUSTOM',
]);

// Email Status
export const emailStatusEnum = pgEnum('email_status', ['PENDING', 'SENT', 'DELIVERED', 'OPENED', 'CLICKED', 'BOUNCED', 'FAILED']);

// Email Templates
export const emailTemplates = pgTable("email_templates", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").references(() => workspaces.id, { onDelete: 'cascade' }),
  
  name: text("name").notNull(),
  subject: text("subject").notNull(),
  htmlContent: text("html_content").notNull(),
  textContent: text("text_content"),
  
  triggerType: emailTriggerTypeEnum("trigger_type"),
  
  isActive: boolean("is_active").notNull().default(true),
  isSystem: boolean("is_system").notNull().default(false),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertEmailTemplateSchema = createInsertSchema(emailTemplates).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertEmailTemplate = z.infer<typeof insertEmailTemplateSchema>;
export type EmailTemplate = typeof emailTemplates.$inferSelect;

// Email Logs - tracking sent emails
export const emailLogs = pgTable("email_logs", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").references(() => workspaces.id, { onDelete: 'cascade' }),
  templateId: varchar("template_id").references(() => emailTemplates.id, { onDelete: 'set null' }),
  
  toEmail: text("to_email").notNull(),
  toName: text("to_name"),
  fromEmail: text("from_email").notNull(),
  fromName: text("from_name"),
  replyTo: text("reply_to"),
  
  subject: text("subject").notNull(),
  htmlContent: text("html_content"),
  textContent: text("text_content"),
  
  status: emailStatusEnum("status").notNull().default('PENDING'),
  
  sendgridMessageId: text("sendgrid_message_id"),
  
  sentAt: timestamp("sent_at"),
  deliveredAt: timestamp("delivered_at"),
  openedAt: timestamp("opened_at"),
  clickedAt: timestamp("clicked_at"),
  bouncedAt: timestamp("bounced_at"),
  failedAt: timestamp("failed_at"),
  
  errorMessage: text("error_message"),
  
  metadata: text("metadata"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertEmailLogSchema = createInsertSchema(emailLogs).omit({ id: true, createdAt: true });
export type InsertEmailLog = z.infer<typeof insertEmailLogSchema>;
export type EmailLog = typeof emailLogs.$inferSelect;

// Password Reset Tokens
export const passwordResetTokens = pgTable("password_reset_tokens", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  userId: varchar("user_id").notNull().references(() => users.id, { onDelete: 'cascade' }),
  
  token: text("token").notNull().unique(),
  
  expiresAt: timestamp("expires_at").notNull(),
  usedAt: timestamp("used_at"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertPasswordResetTokenSchema = createInsertSchema(passwordResetTokens).omit({ id: true, createdAt: true });
export type InsertPasswordResetToken = z.infer<typeof insertPasswordResetTokenSchema>;
export type PasswordResetToken = typeof passwordResetTokens.$inferSelect;

// Email Trigger Type
export type EmailTriggerType = 'WELCOME' | 'PASSWORD_RESET' | 'PROPOSAL_SENT' | 'PROPOSAL_VIEWED' | 'PROPOSAL_ACCEPTED' | 'PROPOSAL_REJECTED' | 'CONTRACT_SENT' | 'CONTRACT_SIGNED' | 'INVOICE_SENT' | 'INVOICE_PAID' | 'INVOICE_OVERDUE' | 'TASK_ASSIGNED' | 'TASK_COMPLETED' | 'PROJECT_CREATED' | 'PROJECT_COMPLETED' | 'DOCUMENT_EXPIRING' | 'BOOKING_CONFIRMED' | 'BOOKING_REMINDER' | 'CUSTOM';

// ============ SMS SYSTEM ============

// SMS Template Trigger Types
export const smsTriggerTypeEnum = pgEnum('sms_trigger_type', [
  'PROPOSAL_SENT',
  'PROPOSAL_ACCEPTED',
  'PROPOSAL_REJECTED',
  'CONTRACT_SENT',
  'CONTRACT_SIGNED',
  'INVOICE_SENT',
  'INVOICE_PAID',
  'INVOICE_OVERDUE',
  'INVOICE_REMINDER',
  'BOOKING_CONFIRMED',
  'BOOKING_REMINDER',
  'CUSTOM',
]);

// SMS Status
export const smsStatusEnum = pgEnum('sms_status', ['PENDING', 'SENT', 'DELIVERED', 'FAILED']);

// SMS Templates
export const smsTemplates = pgTable("sms_templates", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").references(() => workspaces.id, { onDelete: 'cascade' }),
  
  name: text("name").notNull(),
  slug: text("slug").notNull(),
  content: text("content").notNull(),
  category: text("category").notNull().default('TRANSACTIONAL'),
  
  triggerType: smsTriggerTypeEnum("trigger_type"),
  
  isActive: boolean("is_active").notNull().default(true),
  isSystem: boolean("is_system").notNull().default(false),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertSmsTemplateSchema = createInsertSchema(smsTemplates).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertSmsTemplate = z.infer<typeof insertSmsTemplateSchema>;
export type SmsTemplate = typeof smsTemplates.$inferSelect;

// SMS Logs - tracking sent SMS
export const smsLogs = pgTable("sms_logs", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").references(() => workspaces.id, { onDelete: 'cascade' }),
  templateId: varchar("template_id").references(() => smsTemplates.id, { onDelete: 'set null' }),
  
  toPhone: text("to_phone").notNull(),
  toName: text("to_name"),
  fromPhone: text("from_phone").notNull(),
  
  content: text("content").notNull(),
  
  status: smsStatusEnum("status").notNull().default('PENDING'),
  
  twilioMessageId: text("twilio_message_id"),
  
  sentAt: timestamp("sent_at"),
  deliveredAt: timestamp("delivered_at"),
  failedAt: timestamp("failed_at"),
  
  errorMessage: text("error_message"),
  
  metadata: text("metadata"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertSmsLogSchema = createInsertSchema(smsLogs).omit({ id: true, createdAt: true });
export type InsertSmsLog = z.infer<typeof insertSmsLogSchema>;
export type SmsLog = typeof smsLogs.$inferSelect;

// SMS Settings per Workspace
export const smsSettings = pgTable("sms_settings", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().unique().references(() => workspaces.id, { onDelete: 'cascade' }),
  
  smsEnabled: boolean("sms_enabled").notNull().default(false),
  autoInvoiceReminder: boolean("auto_invoice_reminder").notNull().default(false),
  reminderDaysBefore: integer("reminder_days_before").notNull().default(3),
  reminderDaysAfter: integer("reminder_days_after").notNull().default(1),
  
  // Custom Twilio credentials (optional - falls back to global if not set)
  twilioAccountSid: text("twilio_account_sid"),
  twilioAuthToken: text("twilio_auth_token"),
  twilioPhoneNumber: text("twilio_phone_number"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertSmsSettingsSchema = createInsertSchema(smsSettings).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertSmsSettings = z.infer<typeof insertSmsSettingsSchema>;
export type SmsSettings = typeof smsSettings.$inferSelect;

// SMS Trigger Type
export type SmsTriggerType = 'PROPOSAL_SENT' | 'PROPOSAL_ACCEPTED' | 'PROPOSAL_REJECTED' | 'CONTRACT_SENT' | 'CONTRACT_SIGNED' | 'INVOICE_SENT' | 'INVOICE_PAID' | 'INVOICE_OVERDUE' | 'INVOICE_REMINDER' | 'BOOKING_CONFIRMED' | 'BOOKING_REMINDER' | 'CUSTOM';

// ============ WHATSAPP SYSTEM ============

// WhatsApp Template Trigger Types (same as SMS)
export const whatsappTriggerTypeEnum = pgEnum('whatsapp_trigger_type', [
  'PROPOSAL_SENT',
  'PROPOSAL_ACCEPTED',
  'PROPOSAL_REJECTED',
  'CONTRACT_SENT',
  'CONTRACT_SIGNED',
  'INVOICE_SENT',
  'INVOICE_PAID',
  'INVOICE_OVERDUE',
  'INVOICE_REMINDER',
  'BOOKING_CONFIRMED',
  'BOOKING_REMINDER',
  'BOOKING_CANCELLED',
  'BOOKING_RESCHEDULED',
  'TASK_ASSIGNED',
  'TASK_COMPLETED',
  'WELCOME',
  'CUSTOM',
]);

// WhatsApp Message Status
export const whatsappStatusEnum = pgEnum('whatsapp_status', ['PENDING', 'SENT', 'DELIVERED', 'READ', 'FAILED']);

// WhatsApp Message Direction
export const whatsappDirectionEnum = pgEnum('whatsapp_direction', ['OUTBOUND', 'INBOUND']);

// WhatsApp Templates
export const whatsappTemplates = pgTable("whatsapp_templates", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").references(() => workspaces.id, { onDelete: 'cascade' }),
  
  name: text("name").notNull(),
  slug: text("slug").notNull(),
  content: text("content").notNull(),
  category: text("category").notNull().default('TRANSACTIONAL'),
  
  triggerType: whatsappTriggerTypeEnum("trigger_type"),
  
  isActive: boolean("is_active").notNull().default(true),
  isSystem: boolean("is_system").notNull().default(false),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertWhatsappTemplateSchema = createInsertSchema(whatsappTemplates).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertWhatsappTemplate = z.infer<typeof insertWhatsappTemplateSchema>;
export type WhatsappTemplate = typeof whatsappTemplates.$inferSelect;

// WhatsApp Logs - tracking sent/received messages
export const whatsappLogs = pgTable("whatsapp_logs", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").references(() => workspaces.id, { onDelete: 'cascade' }),
  templateId: varchar("template_id").references(() => whatsappTemplates.id, { onDelete: 'set null' }),
  clientId: varchar("client_id").references(() => clients.id, { onDelete: 'set null' }),
  
  direction: whatsappDirectionEnum("direction").notNull().default('OUTBOUND'),
  
  toPhone: text("to_phone").notNull(),
  toName: text("to_name"),
  fromPhone: text("from_phone").notNull(),
  
  content: text("content").notNull(),
  
  status: whatsappStatusEnum("status").notNull().default('PENDING'),
  
  twilioMessageId: text("twilio_message_id"),
  
  sentAt: timestamp("sent_at"),
  deliveredAt: timestamp("delivered_at"),
  readAt: timestamp("read_at"),
  failedAt: timestamp("failed_at"),
  
  errorMessage: text("error_message"),
  
  // For chatbot context
  isFromChatbot: boolean("is_from_chatbot").notNull().default(false),
  chatbotSessionId: varchar("chatbot_session_id"),
  
  metadata: text("metadata"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertWhatsappLogSchema = createInsertSchema(whatsappLogs).omit({ id: true, createdAt: true });
export type InsertWhatsappLog = z.infer<typeof insertWhatsappLogSchema>;
export type WhatsappLog = typeof whatsappLogs.$inferSelect;

// WhatsApp Settings per Workspace
export const whatsappSettings = pgTable("whatsapp_settings", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().unique().references(() => workspaces.id, { onDelete: 'cascade' }),
  
  whatsappEnabled: boolean("whatsapp_enabled").notNull().default(false),
  chatbotEnabled: boolean("chatbot_enabled").notNull().default(false),
  
  // Custom Twilio credentials (optional - falls back to global if not set)
  twilioAccountSid: text("twilio_account_sid"),
  twilioAuthToken: text("twilio_auth_token"),
  twilioPhoneNumber: text("twilio_phone_number"),
  
  // Automatic reminders
  autoInvoiceReminder: boolean("auto_invoice_reminder").notNull().default(false),
  autoBookingReminder: boolean("auto_booking_reminder").notNull().default(false),
  reminderDaysBefore: integer("reminder_days_before").notNull().default(1),
  reminderHoursBefore: integer("reminder_hours_before").notNull().default(24),
  
  // Chatbot settings
  chatbotWelcomeMessage: text("chatbot_welcome_message"),
  chatbotFallbackMessage: text("chatbot_fallback_message"),
  
  // Business info for chatbot context
  businessAddress: text("business_address"),
  businessHours: text("business_hours"), // JSON: {mon: "09:00-18:00", tue: "09:00-18:00", ...}
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertWhatsappSettingsSchema = createInsertSchema(whatsappSettings).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertWhatsappSettings = z.infer<typeof insertWhatsappSettingsSchema>;
export type WhatsappSettings = typeof whatsappSettings.$inferSelect;

// WhatsApp Chatbot Sessions - tracks conversation context
export const whatsappChatbotSessions = pgTable("whatsapp_chatbot_sessions", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  clientId: varchar("client_id").references(() => clients.id, { onDelete: 'set null' }),
  
  phoneNumber: text("phone_number").notNull(),
  
  // Current context/intent
  currentIntent: text("current_intent"), // scheduling, rescheduling, inquiry, etc.
  contextData: text("context_data"), // JSON with conversation state
  
  // Last activity
  lastMessageAt: timestamp("last_message_at").defaultNow().notNull(),
  
  isActive: boolean("is_active").notNull().default(true),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertWhatsappChatbotSessionSchema = createInsertSchema(whatsappChatbotSessions).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertWhatsappChatbotSession = z.infer<typeof insertWhatsappChatbotSessionSchema>;
export type WhatsappChatbotSession = typeof whatsappChatbotSessions.$inferSelect;

// WhatsApp Trigger Type
export type WhatsappTriggerType = 'PROPOSAL_SENT' | 'PROPOSAL_ACCEPTED' | 'PROPOSAL_REJECTED' | 'CONTRACT_SENT' | 'CONTRACT_SIGNED' | 'INVOICE_SENT' | 'INVOICE_PAID' | 'INVOICE_OVERDUE' | 'INVOICE_REMINDER' | 'BOOKING_CONFIRMED' | 'BOOKING_REMINDER' | 'BOOKING_CANCELLED' | 'BOOKING_RESCHEDULED' | 'TASK_ASSIGNED' | 'TASK_COMPLETED' | 'WELCOME' | 'CUSTOM';

// Workspace Google Credentials - for per-workspace Google OAuth
export const workspaceGoogleCredentials = pgTable("workspace_google_credentials", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().unique().references(() => workspaces.id, { onDelete: 'cascade' }),
  
  // OAuth tokens
  accessToken: text("access_token").notNull(),
  refreshToken: text("refresh_token"),
  expiresAt: timestamp("expires_at"),
  
  // Scopes authorized
  scopes: text("scopes"), // comma-separated list of scopes
  
  // Google user info
  googleEmail: text("google_email"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertWorkspaceGoogleCredentialsSchema = createInsertSchema(workspaceGoogleCredentials).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertWorkspaceGoogleCredentials = z.infer<typeof insertWorkspaceGoogleCredentialsSchema>;
export type WorkspaceGoogleCredentials = typeof workspaceGoogleCredentials.$inferSelect;

// Workspace Google Calendar Credentials - for per-workspace Google Calendar OAuth
export const workspaceGoogleCalendarCredentials = pgTable("workspace_google_calendar_credentials", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  workspaceId: varchar("workspace_id").notNull().unique().references(() => workspaces.id, { onDelete: 'cascade' }),
  
  // OAuth tokens
  accessToken: text("access_token").notNull(),
  refreshToken: text("refresh_token"),
  expiresAt: timestamp("expires_at"),
  
  // Scopes authorized
  scopes: text("scopes"),
  
  // Google user info
  googleEmail: text("google_email"),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertWorkspaceGoogleCalendarCredentialsSchema = createInsertSchema(workspaceGoogleCalendarCredentials).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertWorkspaceGoogleCalendarCredentials = z.infer<typeof insertWorkspaceGoogleCalendarCredentialsSchema>;
export type WorkspaceGoogleCalendarCredentials = typeof workspaceGoogleCalendarCredentials.$inferSelect;

// Project Team - Responsibles (assigned members)
export const projectResponsibles = pgTable("project_responsibles", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  projectId: varchar("project_id").notNull().references(() => projects.id, { onDelete: 'cascade' }),
  memberId: varchar("member_id").notNull().references(() => workspaceMembers.id, { onDelete: 'cascade' }),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertProjectResponsibleSchema = createInsertSchema(projectResponsibles).omit({ id: true, createdAt: true });
export type InsertProjectResponsible = z.infer<typeof insertProjectResponsibleSchema>;
export type ProjectResponsible = typeof projectResponsibles.$inferSelect;

// Project Team - Followers (members watching the project)
export const projectFollowers = pgTable("project_followers", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  projectId: varchar("project_id").notNull().references(() => projects.id, { onDelete: 'cascade' }),
  memberId: varchar("member_id").notNull().references(() => workspaceMembers.id, { onDelete: 'cascade' }),
  createdAt: timestamp("created_at").defaultNow().notNull(),
});

export const insertProjectFollowerSchema = createInsertSchema(projectFollowers).omit({ id: true, createdAt: true });
export type InsertProjectFollower = z.infer<typeof insertProjectFollowerSchema>;
export type ProjectFollower = typeof projectFollowers.$inferSelect;

// Project Communications - History of all communications related to a project
export const projectCommunicationChannelEnum = pgEnum("project_communication_channel", ["EMAIL", "SMS", "WHATSAPP", "PHONE", "MEETING", "NOTE"]);
export const projectCommunicationDirectionEnum = pgEnum("project_communication_direction", ["INBOUND", "OUTBOUND"]);
export const projectCommunicationStatusEnum = pgEnum("project_communication_status", ["PENDING", "SENT", "DELIVERED", "FAILED", "READ"]);

export const projectCommunications = pgTable("project_communications", {
  id: varchar("id").primaryKey().default(sql`gen_random_uuid()`),
  projectId: varchar("project_id").notNull().references(() => projects.id, { onDelete: 'cascade' }),
  workspaceId: varchar("workspace_id").notNull().references(() => workspaces.id, { onDelete: 'cascade' }),
  
  // Communication details
  channel: projectCommunicationChannelEnum("channel").notNull(),
  direction: projectCommunicationDirectionEnum("direction").notNull().default('OUTBOUND'),
  subject: text("subject"),
  content: text("content").notNull(),
  
  // Recipient info
  recipientName: text("recipient_name"),
  recipientEmail: text("recipient_email"),
  recipientPhone: text("recipient_phone"),
  
  // Status tracking
  status: projectCommunicationStatusEnum("status").notNull().default('PENDING'),
  sentAt: timestamp("sent_at"),
  deliveredAt: timestamp("delivered_at"),
  readAt: timestamp("read_at"),
  
  // Reference to external message IDs
  externalMessageId: text("external_message_id"),
  
  // Who created this communication
  createdById: varchar("created_by_id").references(() => users.id, { onDelete: 'set null' }),
  
  createdAt: timestamp("created_at").defaultNow().notNull(),
  updatedAt: timestamp("updated_at").defaultNow().notNull(),
});

export const insertProjectCommunicationSchema = createInsertSchema(projectCommunications).omit({ id: true, createdAt: true, updatedAt: true });
export type InsertProjectCommunication = z.infer<typeof insertProjectCommunicationSchema>;
export type ProjectCommunication = typeof projectCommunications.$inferSelect;
