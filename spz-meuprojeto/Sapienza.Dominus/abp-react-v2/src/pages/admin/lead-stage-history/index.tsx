import { Shell } from "@/components/layout/shell";
import { LeadStageHistoryList } from "@/components/lead-stage-history/LeadStageHistoryList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function LeadStageHistoriesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">LeadStageHistories</h1>
              <p className="text-muted-foreground">Manage your leadstagehistories</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/lead-stage-history/create")}>
            <Plus className="size-4" />
            New LeadStageHistory
          </Button>
        </div>

        <LeadStageHistoryList onEdit={(item) => setLocation(`/admin/lead-stage-history/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
