import { Shell } from "@/components/layout/shell";
import { AdvClientes_bkpList } from "@/components/adv-clientes-bkp/AdvClientes_bkpList";
import { AdvClientes_bkpForm } from "@/components/adv-clientes-bkp/AdvClientes_bkpForm";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useState } from "react";

export default function AdvClientes_bkpsPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">advClientes_bkps</h1>
              <p className="text-muted-foreground">Manage your advclientes_bkps</p>
            </div>
          </div>
          <Button className="gap-2" onClick={handleCreate}>
            <Plus className="size-4" />
            New advClientes_bkp
          </Button>
        </div>

        <AdvClientes_bkpList onEdit={handleEdit} />

        <AdvClientes_bkpForm
          isOpen={isFormOpen}
          onClose={() => setIsFormOpen(false)}
          initialValues={selectedItem}
        />
      </div>
    </Shell>
  );
}
