import { Shell } from "@/components/layout/shell";
import { LawyerList } from "@/components/lawyer/LawyerList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function LawyersPage() {
  const [, navigate] = useLocation();

  const handleEdit = (item: any) => {
    navigate(`/admin/lawyer/edit/${item.id}`);
  };

  const handleCreate = () => {
    navigate("/admin/lawyer/create");
  };

  return (
    <Shell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="bg-primary/10 p-2 rounded-lg">
              <Box className="h-6 w-6 text-primary" />
            </div>
            <div>
              <h1 className="text-3xl font-bold tracking-tight">Lawyers</h1>
              <p className="text-muted-foreground">Manage your lawyers</p>
            </div>
          </div>
          <Button className="gap-2" onClick={handleCreate}>
            <Plus className="size-4" />
            New Lawyer
          </Button>
        </div>

        <LawyerList onEdit={handleEdit} />
      </div>
    </Shell>
  );
}
