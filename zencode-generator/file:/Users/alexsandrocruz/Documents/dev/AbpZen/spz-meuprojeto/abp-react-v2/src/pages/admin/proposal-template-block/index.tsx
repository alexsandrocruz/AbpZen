import { Shell } from "@/components/layout/shell";
import { ProposalTemplateBlockList } from "@/components/proposal-template-block/ProposalTemplateBlockList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function ProposalTemplateBlocksPage() {
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
              <h1 className="text-3xl font-bold tracking-tight">ProposalTemplateBlocks</h1>
              <p className="text-muted-foreground">Manage your proposaltemplateblocks</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/proposal-template-block/create")}>
            <Plus className="size-4" />
            New ProposalTemplateBlock
          </Button>
        </div>

        <ProposalTemplateBlockList onEdit={(item) => setLocation(`/admin/proposal-template-block/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
