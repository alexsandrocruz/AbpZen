import { Shell } from "@/components/layout/shell";
import { SitePageList } from "@/components/site-page/SitePageList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function SitePagesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">SitePages</h1>
              <p className="text-muted-foreground">Manage your sitepages</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/site-page/create")}>
            <Plus className="size-4" />
            New SitePage
          </Button>
        </div>

        <SitePageList onEdit={(item) => setLocation(`/admin/site-page/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
