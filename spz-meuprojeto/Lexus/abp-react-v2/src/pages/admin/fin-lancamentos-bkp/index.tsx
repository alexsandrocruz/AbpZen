import { Shell } from "@/components/layout/shell";
import { FinLancamentos_BKPList } from "@/components/fin-lancamentos-bkp/FinLancamentos_BKPList";
import { FinLancamentos_BKPForm } from "@/components/fin-lancamentos-bkp/FinLancamentos_BKPForm";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useState } from "react";

export default function FinLancamentos_BKPsPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">finLancamentos_BKPs</h1>
              <p className="text-muted-foreground">Manage your finlancamentos_bkps</p>
            </div>
          </div>
          <Button className="gap-2" onClick={handleCreate}>
            <Plus className="size-4" />
            New finLancamentos_BKP
          </Button>
        </div>

        <FinLancamentos_BKPList onEdit={handleEdit} />

        <FinLancamentos_BKPForm
          isOpen={isFormOpen}
          onClose={() => setIsFormOpen(false)}
          initialValues={selectedItem}
        />
      </div>
    </Shell>
  );
}
