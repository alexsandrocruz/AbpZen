import { Shell } from "@/components/layout/shell";
import { FabCondicoesPagamentoList } from "@/components/fab-condicoes-pagamento/FabCondicoesPagamentoList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function FabCondicoesPagamentosPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">fabCondicoesPagamentos</h1>
              <p className="text-muted-foreground">Manage your fabcondicoespagamentos</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/fab-condicoes-pagamento/create")}>
            <Plus className="size-4" />
            New fabCondicoesPagamento
          </Button>
        </div>

        <FabCondicoesPagamentoList onEdit={(item) => setLocation(`/admin/fab-condicoes-pagamento/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
