import { Shell } from "@/components/layout/shell";
import { FlwConfigExcecoesList } from "@/components/flw-config-excecoes/FlwConfigExcecoesList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function FlwConfigExcecoesesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">flwConfigExcecoeses</h1>
              <p className="text-muted-foreground">Manage your flwconfigexcecoeses</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/flw-config-excecoes/create")}>
            <Plus className="size-4" />
            New flwConfigExcecoes
          </Button>
        </div>

        <FlwConfigExcecoesList onEdit={(item) => setLocation(`/admin/flw-config-excecoes/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
