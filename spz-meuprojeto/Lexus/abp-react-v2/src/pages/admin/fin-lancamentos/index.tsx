import { Shell } from "@/components/layout/shell";
import { FinLancamentosList } from "@/components/fin-lancamentos/FinLancamentosList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function FinLancamentosesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">finLancamentoses</h1>
              <p className="text-muted-foreground">Manage your finlancamentoses</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/fin-lancamentos/create")}>
            <Plus className="size-4" />
            New finLancamentos
          </Button>
        </div>

        <FinLancamentosList onEdit={(item) => setLocation(`/admin/fin-lancamentos/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
