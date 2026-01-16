import { Shell } from "@/components/layout/shell";
import { AdvClientesINSSStatusList } from "@/components/adv-clientes-inssstatus/AdvClientesINSSStatusList";
import { AdvClientesINSSStatusForm } from "@/components/adv-clientes-inssstatus/AdvClientesINSSStatusForm";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useState } from "react";

export default function AdvClientesINSSStatusesPage() {
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<any>(null);

  const handleCreate = () => {
    setSelectedItem(null);
    setIsFormOpen(true);
  };

  const handleEdit = (item: any) => {
    setSelectedItem(item);
    setIsFormOpen(true);
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
              <h1 className="text-3xl font-bold tracking-tight">advClientesINSSStatuses</h1>
              <p className="text-muted-foreground">Manage your advclientesinssstatuses</p>
            </div>
          </div>
          <Button className="gap-2" onClick={handleCreate}>
            <Plus className="size-4" />
            New advClientesINSSStatus
          </Button>
        </div>

        <AdvClientesINSSStatusList onEdit={handleEdit} />

        <AdvClientesINSSStatusForm
          isOpen={isFormOpen}
          onClose={() => setIsFormOpen(false)}
          initialValues={selectedItem}
        />
      </div>
    </Shell>
  );
}
