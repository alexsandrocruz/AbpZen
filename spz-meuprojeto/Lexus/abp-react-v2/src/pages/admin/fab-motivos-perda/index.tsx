import { Shell } from "@/components/layout/shell";
import { FabMotivosPerdaList } from "@/components/fab-motivos-perda/FabMotivosPerdaList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function FabMotivosPerdasPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">fabMotivosPerdas</h1>
              <p className="text-muted-foreground">Manage your fabmotivosperdas</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/fab-motivos-perda/create")}>
            <Plus className="size-4" />
            New fabMotivosPerda
          </Button>
        </div>

        <FabMotivosPerdaList onEdit={(item) => setLocation(`/admin/fab-motivos-perda/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
