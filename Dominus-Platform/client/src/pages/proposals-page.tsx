import { useState } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useLocation } from "wouter";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Plus, FileText, MoreHorizontal, Pencil, Trash2, ExternalLink, Copy, Printer, Send, FileSignature, LayoutGrid } from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { DeleteModal } from "@/components/modals/delete-modal";
import { GenerateContractModal } from "@/components/modals/generate-contract-modal";
import { ProposalCopilot } from "@/components/ai/proposal-copilot";
import { useToast } from "@/hooks/use-toast";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

export default function ProposalsPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [, navigate] = useLocation();

  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [generateContractModalOpen, setGenerateContractModalOpen] = useState(false);
  const [selectedProposal, setSelectedProposal] = useState<any>(null);

  const { data: proposals = [], isLoading } = useQuery({
    queryKey: ['proposals', currentWorkspace?.id],
    queryFn: () => api.getProposals(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: clients = [] } = useQuery({
    queryKey: ['clients', currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const deleteMutation = useMutation({
    mutationFn: (proposalId: string) => api.deleteProposal(currentWorkspace!.id, proposalId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['proposals', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Proposta excluída com sucesso!" });
      setDeleteModalOpen(false);
      setSelectedProposal(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const getClientName = (id: string) => clients.find((c: any) => c.id === id)?.companyName || 'Desconhecido';

  const statusMap: Record<string, string> = {
    'DRAFT': 'Rascunho',
    'SENT': 'Enviada',
    'ACTIVE': 'Aceita',
    'ARCHIVED': 'Recusada',
  };

  const statusColors: Record<string, string> = {
    'DRAFT': 'bg-gray-50 text-gray-700 border-gray-200',
    'SENT': 'bg-blue-50 text-blue-700 border-blue-200',
    'ACTIVE': 'bg-green-50 text-green-700 border-green-200',
    'ARCHIVED': 'bg-red-50 text-red-700 border-red-200',
  };

  const handleEdit = (proposal: any) => {
    if (currentWorkspace?.slug) {
      navigate(`/${currentWorkspace.slug}/proposals/${proposal.id}/edit`);
    }
  };

  const handleDelete = (proposal: any) => {
    setSelectedProposal(proposal);
    setDeleteModalOpen(true);
  };

  const handleNewProposal = () => {
    if (currentWorkspace?.slug) {
      navigate(`/${currentWorkspace.slug}/proposals/new`);
    }
  };

  const getPublicUrl = (token: string) => {
    return `${window.location.origin}/p/${token}`;
  };

  const handleCopyLink = (proposal: any) => {
    const url = getPublicUrl(proposal.publicToken);
    navigator.clipboard.writeText(url);
    toast({ title: "Link copiado!", description: "O link da proposta foi copiado para a área de transferência." });
  };

  const handleOpenPublic = (proposal: any) => {
    window.open(getPublicUrl(proposal.publicToken), '_blank');
  };

  const handlePrint = (proposal: any) => {
    const url = getPublicUrl(proposal.publicToken);
    const printWindow = window.open(url, '_blank');
    if (printWindow) {
      printWindow.onload = () => {
        setTimeout(() => printWindow.print(), 500);
      };
    }
  };

  const handleGenerateContract = (proposal: any) => {
    setSelectedProposal(proposal);
    setGenerateContractModalOpen(true);
  };

  const createProposalMutation = useMutation({
    mutationFn: async (data: { clientId: string; items: Array<{ description: string; price: number; quantity: number; productId?: string }> }) => {
      const proposal = await api.createProposal(currentWorkspace!.id, {
        clientId: data.clientId,
        title: 'Nova Proposta',
        content: '',
        status: 'DRAFT',
        items: data.items.map(item => ({
          description: item.description,
          price: item.price,
          quantity: item.quantity,
          productId: item.productId,
        })),
      });
      return proposal;
    },
    onSuccess: (proposal) => {
      queryClient.invalidateQueries({ queryKey: ['proposals', currentWorkspace?.id] });
      navigate(`/${currentWorkspace?.slug}/proposals/${proposal.id}/edit`);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleCopilotCreate = (data: { clientId: string; items: Array<{ description: string; price: number; quantity: number; productId?: string }> }) => {
    createProposalMutation.mutate(data);
  };

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Propostas</h1>
            <p className="text-muted-foreground">Crie e acompanhe propostas para clientes.</p>
          </div>
          <div className="flex gap-2">
            <ProposalCopilot onCreateProposal={handleCopilotCreate} />
            <Button className="gap-2" onClick={handleNewProposal} data-testid="button-add-proposal">
              <Plus className="size-4" />
              Nova Proposta
            </Button>
          </div>
        </div>

        {isLoading ? (
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {[...Array(3)].map((_, i) => (
              <Skeleton key={i} className="h-48 rounded-xl" />
            ))}
          </div>
        ) : proposals.length > 0 ? (
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {proposals.map((proposal: any) => (
              <TactileCard key={proposal.id} hover className="p-6 space-y-4 relative group" data-testid={`card-proposal-${proposal.id}`}>
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button variant="ghost" size="icon" className="absolute top-2 right-2 opacity-0 group-hover:opacity-100 transition-opacity">
                      <MoreHorizontal className="size-4" />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                    <DropdownMenuItem onClick={() => handleOpenPublic(proposal)}>
                      <ExternalLink className="size-4 mr-2" />
                      Visualizar
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={() => handleCopyLink(proposal)}>
                      <Copy className="size-4 mr-2" />
                      Copiar Link
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={() => handlePrint(proposal)}>
                      <Printer className="size-4 mr-2" />
                      Imprimir / PDF
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={() => handleGenerateContract(proposal)}>
                      <FileSignature className="size-4 mr-2" />
                      Gerar Contrato
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={() => handleEdit(proposal)}>
                      <Pencil className="size-4 mr-2" />
                      Editar
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={() => navigate(`/${currentWorkspace?.slug}/proposals/${proposal.id}/visual-builder`)}>
                      <LayoutGrid className="size-4 mr-2" />
                      Editor Visual
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={() => handleDelete(proposal)} className="text-destructive">
                      <Trash2 className="size-4 mr-2" />
                      Excluir
                    </DropdownMenuItem>
                  </DropdownMenuContent>
                </DropdownMenu>

                <div className="flex items-start gap-3">
                  <div className="size-10 rounded-lg bg-primary/10 flex items-center justify-center">
                    <FileText className="size-5 text-primary" />
                  </div>
                  <div className="flex-1">
                    <h3 className="font-semibold">{proposal.title}</h3>
                    <p className="text-sm text-muted-foreground">{getClientName(proposal.clientId)}</p>
                  </div>
                </div>

                <p className="text-sm text-muted-foreground line-clamp-2">
                  {proposal.content}
                </p>

                <div className="flex items-center justify-between pt-2">
                  <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium border ${statusColors[proposal.status] || 'bg-gray-50 text-gray-700 border-gray-200'}`}>
                    {statusMap[proposal.status] || proposal.status}
                  </span>
                  <span className="text-xs text-muted-foreground">
                    {format(new Date(proposal.createdAt), "d 'de' MMM", { locale: ptBR })}
                  </span>
                </div>
              </TactileCard>
            ))}
          </div>
        ) : (
          <TactileCard className="min-h-[400px] flex flex-col items-center justify-center text-center p-8 space-y-6 border-dashed">
            <div className="size-20 rounded-full bg-primary/5 flex items-center justify-center">
              <FileText className="size-10 text-primary/40" />
            </div>
            <div className="max-w-md space-y-2">
              <h3 className="text-xl font-semibold">Nenhuma proposta ainda</h3>
              <p className="text-muted-foreground">Crie propostas profissionais com contratos embutidos e assinaturas digitais. Envie a sua primeira hoje.</p>
            </div>
            <Button variant="outline" onClick={handleNewProposal}>Criar Proposta</Button>
          </TactileCard>
        )}
      </div>

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteMutation.mutate(selectedProposal?.id)}
        title="Excluir Proposta"
        description={`Tem certeza que deseja excluir a proposta "${selectedProposal?.title}"?`}
        isLoading={deleteMutation.isPending}
      />

      <GenerateContractModal
        open={generateContractModalOpen}
        onOpenChange={setGenerateContractModalOpen}
        proposal={selectedProposal}
      />
    </AppShell>
  );
}
