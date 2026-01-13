import { Shell } from "@/components/layout/shell";
import { AdvPreMotivosPerdaList } from "@/components/adv-pre-motivos-perda/AdvPreMotivosPerdaList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function AdvPreMotivosPerdasPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">advPreMotivosPerdas</h1>
              <p className="text-muted-foreground">Manage your advpremotivosperdas</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/adv-pre-motivos-perda/create")}>
            <Plus className="size-4" />
            New advPreMotivosPerda
          </Button>
        </div>

        <AdvPreMotivosPerdaList onEdit={(item) => setLocation(`/admin/adv-pre-motivos-perda/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
