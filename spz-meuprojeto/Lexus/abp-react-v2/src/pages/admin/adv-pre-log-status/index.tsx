import { Shell } from "@/components/layout/shell";
import { AdvPreLogStatusList } from "@/components/adv-pre-log-status/AdvPreLogStatusList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function AdvPreLogStatusesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">advPreLogStatuses</h1>
              <p className="text-muted-foreground">Manage your advprelogstatuses</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/adv-pre-log-status/create")}>
            <Plus className="size-4" />
            New advPreLogStatus
          </Button>
        </div>

        <AdvPreLogStatusList onEdit={(item) => setLocation(`/admin/adv-pre-log-status/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
