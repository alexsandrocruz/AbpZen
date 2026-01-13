import { Shell } from "@/components/layout/shell";
import { FabFormasRecebimentoList } from "@/components/fab-formas-recebimento/FabFormasRecebimentoList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function FabFormasRecebimentosPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">fabFormasRecebimentos</h1>
              <p className="text-muted-foreground">Manage your fabformasrecebimentos</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/fab-formas-recebimento/create")}>
            <Plus className="size-4" />
            New fabFormasRecebimento
          </Button>
        </div>

        <FabFormasRecebimentoList onEdit={(item) => setLocation(`/admin/fab-formas-recebimento/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
