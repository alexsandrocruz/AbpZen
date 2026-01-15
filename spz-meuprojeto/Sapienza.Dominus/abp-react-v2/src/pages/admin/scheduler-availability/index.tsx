import { Shell } from "@/components/layout/shell";
import { SchedulerAvailabilityList } from "@/components/scheduler-availability/SchedulerAvailabilityList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function SchedulerAvailabilitiesPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">SchedulerAvailabilities</h1>
              <p className="text-muted-foreground">Manage your scheduleravailabilities</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/scheduler-availability/create")}>
            <Plus className="size-4" />
            New SchedulerAvailability
          </Button>
        </div>

        <SchedulerAvailabilityList onEdit={(item) => setLocation(`/admin/scheduler-availability/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
