import { Shell } from "@/components/layout/shell";
import { AdvProVarasList } from "@/components/adv-pro-varas/AdvProVarasList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function AdvProVarasesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">advProVarases</h1>
              <p className="text-muted-foreground">Manage your advprovarases</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/adv-pro-varas/create")}>
            <Plus className="size-4" />
            New advProVaras
          </Button>
        </div>

        <AdvProVarasList onEdit={(item) => setLocation(`/admin/adv-pro-varas/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
