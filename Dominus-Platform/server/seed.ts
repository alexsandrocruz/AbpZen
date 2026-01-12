import { storage } from "./storage";
import { scrypt, randomBytes } from "crypto";
import { promisify } from "util";
import { addDays, subDays } from "date-fns";
import { seedEmailTemplates } from "./email-seed";
import { initializeSystemSmsTemplates } from "./sms-service";
import { initializeSystemWhatsappTemplates } from "./whatsapp-service";

const scryptAsync = promisify(scrypt);

async function hashPassword(password: string) {
  const salt = randomBytes(16).toString("hex");
  const buf = (await scryptAsync(password, salt, 64)) as Buffer;
  return `${buf.toString("hex")}.${salt}`;
}

async function seed() {
  console.log("🌱 Seeding database...");

  try {
    // Create demo user
    const hashedPassword = await hashPassword("password123");
    const user = await storage.createUser({
      email: "demo@dominus.app",
      password: hashedPassword,
      name: "Alex Founder",
      avatar: "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80",
    });
    console.log("✅ Created demo user:", user.email);

    // Create workspace
    const workspace = await storage.createWorkspace({
      name: "Acme Studio",
      slug: "acme-studio",
      ownerId: user.id,
    });
    console.log("✅ Created workspace:", workspace.name);

    // Add user as owner
    await storage.addWorkspaceMember({
      workspaceId: workspace.id,
      userId: user.id,
      role: "OWNER",
    });
    console.log("✅ Added user as workspace owner");

    // Create clients
    const client1 = await storage.createClient({
      workspaceId: workspace.id,
      name: "Sarah Miller",
      email: "sarah@globex.com",
      companyName: "Globex Corp",
      avatar: "https://images.unsplash.com/photo-1494790108377-be9c29b29330?ixlib=rb-1.2.1&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80",
      status: "ACTIVE",
    });

    const client2 = await storage.createClient({
      workspaceId: workspace.id,
      name: "David Chen",
      email: "david@soylent.com",
      companyName: "Soylent Corp",
      avatar: "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?ixlib=rb-1.2.1&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80",
      status: "ACTIVE",
    });
    console.log("✅ Created 2 clients");

    // Create projects
    const project1 = await storage.createProject({
      workspaceId: workspace.id,
      clientId: client1.id,
      title: "Website Redesign",
      description: "Complete redesign of corporate website",
      status: "IN_PROGRESS",
      budget: "5000",
      dueDate: addDays(new Date(), 14),
    });

    const project2 = await storage.createProject({
      workspaceId: workspace.id,
      clientId: client2.id,
      title: "Mobile App MVP",
      description: "Build initial version of mobile application",
      status: "PENDING",
      budget: "12000",
      dueDate: addDays(new Date(), 30),
    });
    console.log("✅ Created 2 projects");

    // Create tasks
    await storage.createTask({
      workspaceId: workspace.id,
      projectId: project1.id,
      title: "Design Homepage Mockups",
      description: "Create initial design concepts for homepage",
      isCompleted: true,
      assigneeId: user.id,
      dueDate: subDays(new Date(), 2),
    });

    await storage.createTask({
      workspaceId: workspace.id,
      projectId: project1.id,
      title: "Client Feedback Review",
      description: "Review and incorporate client feedback",
      isCompleted: false,
      assigneeId: user.id,
      dueDate: new Date(),
    });

    await storage.createTask({
      workspaceId: workspace.id,
      projectId: project1.id,
      title: "Frontend Implementation",
      description: "Implement responsive frontend",
      isCompleted: false,
      assigneeId: user.id,
      dueDate: addDays(new Date(), 5),
    });
    console.log("✅ Created 3 tasks");

    // Create invoices
    const invoice1 = await storage.createInvoice({
      workspaceId: workspace.id,
      clientId: client1.id,
      projectId: project1.id,
      number: "INV-2024-001",
      issueDate: subDays(new Date(), 10),
      dueDate: addDays(new Date(), 4),
      status: "SENT",
      notes: "50% deposit for website redesign project",
    });

    await storage.createInvoiceItem({
      invoiceId: invoice1.id,
      description: "Deposit - Website Redesign",
      quantity: 1,
      price: "2500",
    });

    const invoice2 = await storage.createInvoice({
      workspaceId: workspace.id,
      clientId: client1.id,
      projectId: project1.id,
      number: "INV-2024-002",
      issueDate: subDays(new Date(), 2),
      dueDate: addDays(new Date(), 12),
      status: "DRAFT",
      notes: "Final payment upon completion",
    });

    await storage.createInvoiceItem({
      invoiceId: invoice2.id,
      description: "Final Payment - Website Redesign",
      quantity: 1,
      price: "2500",
    });
    console.log("✅ Created 2 invoices with line items");

    console.log("\n🎉 Seed completed successfully!");
    console.log("\n📝 Demo credentials:");
    console.log("   Email: demo@dominus.app");
    console.log("   Password: password123");
    console.log("   Workspace: acme-studio\n");

    process.exit(0);
  } catch (error) {
    console.error("❌ Seed failed:", error);
    process.exit(1);
  }
}

export async function seedSystemData() {
  console.log("🌱 Seeding system data...");
  
  try {
    await seedEmailTemplates();
    await initializeSystemSmsTemplates();
    await initializeSystemWhatsappTemplates();
    console.log("\n🎉 System seed completed successfully!");
  } catch (error) {
    console.error("❌ System seed failed:", error);
    throw error;
  }
}

const args = process.argv.slice(2);
if (args.includes('--system')) {
  seedSystemData()
    .then(() => process.exit(0))
    .catch(() => process.exit(1));
} else if (import.meta.url === `file://${process.argv[1]}`) {
  seed();
}
