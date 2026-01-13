import { Shell } from "@/components/layout/shell";
import { AdvAgeTiposTarefasList } from "@/components/adv-age-tipos-tarefas/AdvAgeTiposTarefasList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function AdvAgeTiposTarefasesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">advAgeTiposTarefases</h1>
              <p className="text-muted-foreground">Manage your advagetipostarefases</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/adv-age-tipos-tarefas/create")}>
            <Plus className="size-4" />
            New advAgeTiposTarefas
          </Button>
        </div>

        <AdvAgeTiposTarefasList onEdit={(item) => setLocation(`/admin/adv-age-tipos-tarefas/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
