import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { addDays, subDays } from 'date-fns';

// Types
export type Role = 'OWNER' | 'MEMBER' | 'CLIENT';
export type Status = 'ACTIVE' | 'ARCHIVED' | 'DRAFT' | 'SENT' | 'PAID' | 'OVERDUE' | 'COMPLETED' | 'IN_PROGRESS' | 'PENDING';

export interface User {
  id: string;
  name: string;
  email: string;
  avatar?: string;
}

export interface Member {
  userId: string;
  role: Role;
}

export interface Client {
  id: string;
  name: string;
  email: string;
  companyName: string;
  avatar?: string;
  status: Status;
}

export interface Project {
  id: string;
  title: string;
  clientId: string;
  status: Status;
  dueDate: Date;
  budget: number;
}

export interface Task {
  id: string;
  projectId: string;
  title: string;
  isCompleted: boolean;
  assigneeId?: string;
  dueDate?: Date;
}

export interface Invoice {
  id: string;
  clientId: string;
  projectId?: string;
  number: string;
  issueDate: Date;
  dueDate: Date;
  status: Status;
  total: number;
  items: { description: string; quantity: number; price: number }[];
}

export interface Workspace {
  id: string;
  slug: string;
  name: string;
  ownerId: string;
  members: Member[];
}

// Initial Data
const currentUser: User = {
  id: 'u1',
  name: 'Alex Founder',
  email: 'alex@dominus.app',
  avatar: 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80',
};

const currentWorkspace: Workspace = {
  id: 'w1',
  slug: 'acme-studio',
  name: 'Acme Studio',
  ownerId: 'u1',
  members: [{ userId: 'u1', role: 'OWNER' }],
};

const clients: Client[] = [
  { id: 'c1', name: 'Sarah Miller', email: 'sarah@globex.com', companyName: 'Globex Corp', status: 'ACTIVE', avatar: 'https://images.unsplash.com/photo-1494790108377-be9c29b29330?ixlib=rb-1.2.1&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80' },
  { id: 'c2', name: 'David Chen', email: 'david@soylent.com', companyName: 'Soylent Corp', status: 'ACTIVE', avatar: 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?ixlib=rb-1.2.1&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80' },
];

const projects: Project[] = [
  { id: 'p1', title: 'Website Redesign', clientId: 'c1', status: 'IN_PROGRESS', dueDate: addDays(new Date(), 14), budget: 5000 },
  { id: 'p2', title: 'Mobile App MVP', clientId: 'c2', status: 'PENDING', dueDate: addDays(new Date(), 30), budget: 12000 },
];

const tasks: Task[] = [
  { id: 't1', projectId: 'p1', title: 'Design Homepage Mockups', isCompleted: true, assigneeId: 'u1', dueDate: subDays(new Date(), 2) },
  { id: 't2', projectId: 'p1', title: 'Client Feedback Review', isCompleted: false, assigneeId: 'u1', dueDate: new Date() },
  { id: 't3', projectId: 'p1', title: 'Frontend Implementation', isCompleted: false, assigneeId: 'u1', dueDate: addDays(new Date(), 5) },
];

const invoices: Invoice[] = [
  { 
    id: 'inv1', 
    clientId: 'c1', 
    projectId: 'p1', 
    number: 'INV-2024-001', 
    issueDate: subDays(new Date(), 10), 
    dueDate: addDays(new Date(), 4), 
    status: 'SENT', 
    total: 2500,
    items: [{ description: 'Deposit - Website Redesign', quantity: 1, price: 2500 }] 
  },
  { 
    id: 'inv2', 
    clientId: 'c1', 
    projectId: 'p1', 
    number: 'INV-2024-002', 
    issueDate: subDays(new Date(), 2), 
    dueDate: addDays(new Date(), 12), 
    status: 'DRAFT', 
    total: 2500,
    items: [{ description: 'Final Payment - Website Redesign', quantity: 1, price: 2500 }] 
  },
];

// Store
interface DominusState {
  user: User | null;
  workspace: Workspace | null;
  clients: Client[];
  projects: Project[];
  tasks: Task[];
  invoices: Invoice[];
  
  // Actions
  login: () => void;
  logout: () => void;
  addClient: (client: Omit<Client, 'id' | 'status'>) => void;
  addProject: (project: Omit<Project, 'id' | 'status'>) => void;
  toggleTask: (id: string) => void;
  addTask: (task: Omit<Task, 'id' | 'isCompleted'>) => void;
}

export const useDominus = create<DominusState>()(
  persist(
    (set) => ({
      user: null, // Start logged out for demo flow
      workspace: currentWorkspace,
      clients: clients,
      projects: projects,
      tasks: tasks,
      invoices: invoices,

      login: () => set({ user: currentUser }),
      logout: () => set({ user: null }),
      
      addClient: (newClient) => set((state) => ({
        clients: [...state.clients, { ...newClient, id: Math.random().toString(36).substr(2, 9), status: 'ACTIVE' }]
      })),
      
      addProject: (newProject) => set((state) => ({
        projects: [...state.projects, { ...newProject, id: Math.random().toString(36).substr(2, 9), status: 'PENDING' }]
      })),
      
      toggleTask: (id) => set((state) => ({
        tasks: state.tasks.map(t => t.id === id ? { ...t, isCompleted: !t.isCompleted } : t)
      })),
      
      addTask: (newTask) => set((state) => ({
        tasks: [...state.tasks, { ...newTask, id: Math.random().toString(36).substr(2, 9), isCompleted: false }]
      })),
    }),
    {
      name: 'dominus-storage',
    }
  )
);
