import { Shell } from "@/components/layout/shell";
import { LeadLandingPageList } from "@/components/lead-landing-page/LeadLandingPageList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function LeadLandingPagesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">LeadLandingPages</h1>
              <p className="text-muted-foreground">Manage your leadlandingpages</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/lead-landing-page/create")}>
            <Plus className="size-4" />
            New LeadLandingPage
          </Button>
        </div>

        <LeadLandingPageList onEdit={(item) => setLocation(`/admin/lead-landing-page/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
