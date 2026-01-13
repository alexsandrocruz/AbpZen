import { Shell } from "@/components/layout/shell";
import { FinGruposDREList } from "@/components/fin-grupos-dre/FinGruposDREList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function FinGruposDREsPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">finGruposDREs</h1>
              <p className="text-muted-foreground">Manage your fingruposdres</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/fin-grupos-dre/create")}>
            <Plus className="size-4" />
            New finGruposDRE
          </Button>
        </div>

        <FinGruposDREList onEdit={(item) => setLocation(`/admin/fin-grupos-dre/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
