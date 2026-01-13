import { Shell } from "@/components/layout/shell";
import { OpoOportunidadesList } from "@/components/opo-oportunidades/OpoOportunidadesList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function OpoOportunidadesesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">opoOportunidadeses</h1>
              <p className="text-muted-foreground">Manage your opooportunidadeses</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/opo-oportunidades/create")}>
            <Plus className="size-4" />
            New opoOportunidades
          </Button>
        </div>

        <OpoOportunidadesList onEdit={(item) => setLocation(`/admin/opo-oportunidades/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
