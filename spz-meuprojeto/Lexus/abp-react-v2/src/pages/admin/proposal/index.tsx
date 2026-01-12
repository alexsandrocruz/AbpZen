import { Shell } from "@/components/layout/shell";
import { ProposalList } from "@/components/proposal/ProposalList";
import { ProposalForm } from "@/components/proposal/ProposalForm";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useState } from "react";

export default function ProposalsPage() {
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingItem, setEditingItem] = useState<any>(null);

  const handleEdit = (item: any) => {
    setEditingItem(item);
    setIsFormOpen(true);
  };

  const handleCloseForm = () => {
    setIsFormOpen(false);
    setEditingItem(null);
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
              <h1 className="text-3xl font-bold tracking-tight">Proposals</h1>
              <p className="text-muted-foreground">Manage your proposals</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setIsFormOpen(true)}>
            <Plus className="size-4" />
            New Proposal
          </Button>
        </div>

        <ProposalList onEdit={handleEdit} />

        <ProposalForm 
          isOpen={isFormOpen} 
          onClose={handleCloseForm} 
          initialValues={editingItem} 
        />
      </div>
    </Shell>
  );
}
