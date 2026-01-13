import { Shell } from "@/components/layout/shell";
import { AdvCliLogList } from "@/components/adv-cli-log/AdvCliLogList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function AdvCliLogsPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">advCliLogs</h1>
              <p className="text-muted-foreground">Manage your advclilogs</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/adv-cli-log/create")}>
            <Plus className="size-4" />
            New advCliLog
          </Button>
        </div>

        <AdvCliLogList onEdit={(item) => setLocation(`/admin/adv-cli-log/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
