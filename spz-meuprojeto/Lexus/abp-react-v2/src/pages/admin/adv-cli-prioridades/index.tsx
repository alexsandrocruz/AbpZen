import { Shell } from "@/components/layout/shell";
import { AdvCliPrioridadesList } from "@/components/adv-cli-prioridades/AdvCliPrioridadesList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function AdvCliPrioridadesesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">advCliPrioridadeses</h1>
              <p className="text-muted-foreground">Manage your advcliprioridadeses</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/adv-cli-prioridades/create")}>
            <Plus className="size-4" />
            New advCliPrioridades
          </Button>
        </div>

        <AdvCliPrioridadesList onEdit={(item) => setLocation(`/admin/adv-cli-prioridades/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
