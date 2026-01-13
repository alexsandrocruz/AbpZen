import { Shell } from "@/components/layout/shell";
import { AdvPreCheckListsGruposList } from "@/components/adv-pre-check-lists-grupos/AdvPreCheckListsGruposList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function AdvPreCheckListsGruposesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">advPreCheckListsGruposes</h1>
              <p className="text-muted-foreground">Manage your advprechecklistsgruposes</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/adv-pre-check-lists-grupos/create")}>
            <Plus className="size-4" />
            New advPreCheckListsGrupos
          </Button>
        </div>

        <AdvPreCheckListsGruposList onEdit={(item) => setLocation(`/admin/adv-pre-check-lists-grupos/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
