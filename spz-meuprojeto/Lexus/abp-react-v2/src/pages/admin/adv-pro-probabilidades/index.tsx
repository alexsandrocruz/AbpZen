import { Shell } from "@/components/layout/shell";
import { AdvProProbabilidadesList } from "@/components/adv-pro-probabilidades/AdvProProbabilidadesList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function AdvProProbabilidadesesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">advProProbabilidadeses</h1>
              <p className="text-muted-foreground">Manage your advproprobabilidadeses</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/adv-pro-probabilidades/create")}>
            <Plus className="size-4" />
            New advProProbabilidades
          </Button>
        </div>

        <AdvProProbabilidadesList onEdit={(item) => setLocation(`/admin/adv-pro-probabilidades/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
