import { Shell } from "@/components/layout/shell";
import { ArtistSpecialtyList } from "@/components/artist-specialty/ArtistSpecialtyList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function ArtistSpecialtiesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">ArtistSpecialties</h1>
              <p className="text-muted-foreground">Manage your artistspecialties</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/artist-specialty/create")}>
            <Plus className="size-4" />
            New ArtistSpecialty
          </Button>
        </div>

        <ArtistSpecialtyList onEdit={(item) => setLocation(`/admin/artist-specialty/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
