import { Shell } from "@/components/layout/shell";
import { ProjectCommunicationList } from "@/components/project-communication/ProjectCommunicationList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function ProjectCommunicationsPage() {
  const [, setLocation] = useLocation();

  return (
    <Shell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="bg-primary/10 p-2 rounded-lg">
              <Box className="h-6 w-6 text-primary" />
            </div>
            <div>
              <h1 className="text-3xl font-bold tracking-tight">ProjectCommunications</h1>
              <p className="text-muted-foreground">Manage your projectcommunications</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/project-communication/create")}>
            <Plus className="size-4" />
            New ProjectCommunication
          </Button>
        </div>

        <ProjectCommunicationList onEdit={(item) => setLocation(`/admin/project-communication/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
