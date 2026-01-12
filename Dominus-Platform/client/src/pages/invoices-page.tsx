import { useState } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Plus, MoreHorizontal, Pencil, Trash2 } from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { InvoiceModal } from "@/components/modals/invoice-modal";
import { DeleteModal } from "@/components/modals/delete-modal";
import { useToast } from "@/hooks/use-toast";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

export default function InvoicesPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedInvoice, setSelectedInvoice] = useState<any>(null);

  const { data: invoices = [], isLoading } = useQuery({
    queryKey: ['invoices', currentWorkspace?.id],
    queryFn: () => api.getInvoices(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: clients = [] } = useQuery({
    queryKey: ['clients', currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const deleteMutation = useMutation({
    mutationFn: (invoiceId: string) => api.deleteInvoice(currentWorkspace!.id, invoiceId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['invoices', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Fatura excluída com sucesso!" });
      setDeleteModalOpen(false);
      setSelectedInvoice(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const getClientName = (id: string) => {
    const client = clients.find((c: any) => c.id === id);
    return client?.companyName || client?.name || 'Desconhecido';
  };

  const statusMap: Record<string, string> = {
    'DRAFT': 'Rascunho',
    'SENT': 'Enviada',
    'PAID': 'Paga',
    'OVERDUE': 'Atrasada',
  };

  const handleEdit = (invoice: any) => {
    setSelectedInvoice(invoice);
    setModalOpen(true);
  };

  const handleDelete = (invoice: any) => {
    setSelectedInvoice(invoice);
    setDeleteModalOpen(true);
  };

  const handleNewInvoice = () => {
    setSelectedInvoice(null);
    setModalOpen(true);
  };

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Faturas</h1>
            <p className="text-muted-foreground">Gerencie pagamentos e cobranças.</p>
          </div>
          <Button className="gap-2" onClick={handleNewInvoice} data-testid="button-add-invoice">
            <Plus className="size-4" />
            Criar Fatura
          </Button>
        </div>

        {isLoading ? (
          <Skeleton className="h-64 rounded-xl" />
        ) : (
          <TactileCard className="overflow-hidden">
            <Table>
              <TableHeader className="bg-muted/50">
                <TableRow>
                  <TableHead className="w-[100px]">Número</TableHead>
                  <TableHead>Cliente</TableHead>
                  <TableHead>Data de Emissão</TableHead>
                  <TableHead>Vencimento</TableHead>
                  <TableHead>Status</TableHead>
                  <TableHead className="text-right">Valor</TableHead>
                  <TableHead className="w-[50px]"></TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {invoices.map((invoice: any) => (
                  <TableRow key={invoice.id} className="hover:bg-muted/50" data-testid={`row-invoice-${invoice.id}`}>
                    <TableCell className="font-medium">{invoice.number}</TableCell>
                    <TableCell>{getClientName(invoice.clientId)}</TableCell>
                    <TableCell>{format(new Date(invoice.issueDate), "d 'de' MMM, yyyy", { locale: ptBR })}</TableCell>
                    <TableCell>{format(new Date(invoice.dueDate), "d 'de' MMM, yyyy", { locale: ptBR })}</TableCell>
                    <TableCell>
                      <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium border ${
                        invoice.status === 'PAID' ? 'bg-green-50 text-green-700 border-green-200' :
                        invoice.status === 'SENT' ? 'bg-blue-50 text-blue-700 border-blue-200' :
                        invoice.status === 'OVERDUE' ? 'bg-red-50 text-red-700 border-red-200' :
                        'bg-gray-50 text-gray-700 border-gray-200'
                      }`}>
                        {statusMap[invoice.status] || invoice.status}
                      </span>
                    </TableCell>
                    <TableCell className="text-right font-medium">R$ {parseFloat(invoice.total || '0').toLocaleString('pt-BR')}</TableCell>
                    <TableCell>
                      <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                          <Button variant="ghost" size="icon" className="size-8">
                            <MoreHorizontal className="size-4" />
                          </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                          <DropdownMenuItem onClick={() => handleEdit(invoice)}>
                            <Pencil className="size-4 mr-2" />
                            Editar
                          </DropdownMenuItem>
                          <DropdownMenuItem onClick={() => handleDelete(invoice)} className="text-destructive">
                            <Trash2 className="size-4 mr-2" />
                            Excluir
                          </DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </TableCell>
                  </TableRow>
                ))}
                {invoices.length === 0 && (
                  <TableRow>
                    <TableCell colSpan={7} className="h-24 text-center text-muted-foreground">
                      Nenhuma fatura ainda. Crie sua primeira fatura!
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </TactileCard>
        )}
      </div>

      <InvoiceModal
        open={modalOpen}
        onOpenChange={setModalOpen}
        invoice={selectedInvoice}
      />

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteMutation.mutate(selectedInvoice?.id)}
        title="Excluir Fatura"
        description={`Tem certeza que deseja excluir a fatura "${selectedInvoice?.number}"?`}
        isLoading={deleteMutation.isPending}
      />
    </AppShell>
  );
}
