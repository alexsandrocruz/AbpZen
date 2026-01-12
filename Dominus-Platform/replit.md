# Dominus - Business Management Platform

## Overview

Dominus is a full-stack business management application designed for freelancers and agencies. It provides workspace-based organization for managing clients, projects, tasks, proposals, and invoices. The application uses a React frontend with Express backend, PostgreSQL database, and follows a multi-tenant architecture where each workspace operates independently.

## User Preferences

Preferred communication style: Simple, everyday language.

## System Architecture

### Frontend Architecture
- **Framework**: React 18 with TypeScript
- **Routing**: Wouter (lightweight React router)
- **State Management**: Zustand for global state with persistence middleware
- **Data Fetching**: TanStack React Query for server state management
- **UI Components**: shadcn/ui component library built on Radix UI primitives
- **Styling**: Tailwind CSS v4 with custom design tokens and CSS variables
- **Build Tool**: Vite with custom plugins for Replit integration

### Backend Architecture
- **Runtime**: Node.js with Express
- **Language**: TypeScript with ESM modules
- **Authentication**: Passport.js with local strategy, session-based auth using express-session
- **Password Security**: scrypt-based hashing with timing-safe comparison
- **API Design**: RESTful endpoints organized by resource type

### Data Storage
- **Database**: PostgreSQL
- **ORM**: Drizzle ORM with drizzle-zod for schema validation
- **Schema Location**: `shared/schema.ts` contains all table definitions
- **Migrations**: Managed via `drizzle-kit push` command

### Multi-Tenant Design
- Workspaces serve as the tenant boundary
- All business entities (clients, projects, tasks, invoices) belong to a workspace
- Users can belong to multiple workspaces with different roles (OWNER, MEMBER, CLIENT)
- URL structure uses workspace slug for routing: `/:slug/dashboard`

### Authentication Flow
- Session-based authentication with memory store (MemoryStore)
- Protected routes check authentication status via middleware
- Frontend uses Zustand store with localStorage persistence for auth state

### Build Process
- Client builds with Vite to `dist/public`
- Server bundles with esbuild, selectively bundling dependencies for faster cold starts
- Combined output enables single-command production deployment

### Key Features

#### Proposal-to-Contract Conversion
- Generate contracts directly from proposals with flexible payment scheduling
- Three payment schedule types: EQUAL (parcelas iguais), PERCENTAGE (percentual), MANUAL (manual)
- Automatic installment calculation with configurable frequency (mensal, semanal, quinzenal)
- Optional automatic transaction generation for each installment
- Contract items copied from proposal items
- Modal wizard: GenerateContractModal with 3-step process

#### Recurring Transactions
- Create recurring financial transactions with multiple frequencies
- Supports MONTHLY, WEEKLY, BIWEEKLY, YEARLY recurrence types
- Batch creation for efficient database operations

#### Custom Fields System
- Define custom fields per entity type (CLIENT, PROJECT, TASK, etc.)
- Supports TEXT, NUMBER, DATE, CHECKBOX, SELECT field types
- Dynamic rendering in entity forms

#### Email System
- SendGrid integration for transactional emails
- 18 system email templates for workflow events (WELCOME, PASSWORD_RESET, PROPOSAL_SENT, etc.)
- Templates stored in database with workspace scoping
- System templates (isSystem: true, workspaceId: null) are global and read-only
- Auto-seed on server startup ensures templates exist in production
- Seed files: `server/email-seed.ts` (templates), `server/seed.ts` (orchestrator)
- Email tracking: opens (1x1 pixel GIF), clicks (redirect proxy)
- Template variables: {{nome}}, {{email}}, {{workspace}}, {{link}}, {{data}}

## External Dependencies

### Database
- **PostgreSQL**: Primary data store, connection via `DATABASE_URL` environment variable
- **connect-pg-simple**: Session storage option (currently using memory store)

### UI Libraries
- **Radix UI**: Full suite of accessible, unstyled primitives
- **Lucide React**: Icon library
- **date-fns**: Date manipulation utilities
- **react-day-picker**: Calendar component

### Validation
- **Zod**: Schema validation
- **drizzle-zod**: Auto-generates Zod schemas from Drizzle tables
- **@hookform/resolvers**: Connects Zod to React Hook Form

### Development Tools
- **Replit Vite Plugins**: Runtime error overlay, cartographer, dev banner
- **tsx**: TypeScript execution for development