import { Shell } from "@/components/layout/shell";
import { AutoFTPList } from "@/components/auto-ftp/AutoFTPList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function AutoFTPsPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">autoFTPs</h1>
              <p className="text-muted-foreground">Manage your autoftps</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/auto-ftp/create")}>
            <Plus className="size-4" />
            New autoFTP
          </Button>
        </div>

        <AutoFTPList onEdit={(item) => setLocation(`/admin/auto-ftp/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
