import { useState, useEffect } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Skeleton } from "@/components/ui/skeleton";
import { Badge } from "@/components/ui/badge";
import { Plus, Eye, Building2, User, MoreHorizontal, Pencil, Trash2, Users, Phone, X, Mail, Search, ChevronLeft, ChevronRight, ArrowUpDown } from "lucide-react";
import { Link } from "wouter";
import { ClientModal } from "@/components/modals/client-modal";
import { DeleteModal } from "@/components/modals/delete-modal";
import { useToast } from "@/hooks/use-toast";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Loader2 } from "lucide-react";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Checkbox } from "@/components/ui/checkbox";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

function formatCPF(cpf: string): string {
  if (!cpf) return '';
  const digits = cpf.replace(/\D/g, '');
  return digits
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d{1,2})$/, '$1-$2');
}

function formatCNPJ(cnpj: string): string {
  if (!cnpj) return '';
  const digits = cnpj.replace(/\D/g, '');
  return digits
    .replace(/(\d{2})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1/$2')
    .replace(/(\d{4})(\d{1,2})$/, '$1-$2');
}

interface ContactsModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  client: any;
}

function ContactsModal({ open, onOpenChange, client }: ContactsModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  
  const [showAddForm, setShowAddForm] = useState(false);
  const [editingContact, setEditingContact] = useState<any>(null);
  const [formData, setFormData] = useState({
    name: "",
    role: "",
    email: "",
    phone: "",
    whatsapp: "",
    isPrimary: false,
    notes: "",
  });

  const { data: contacts = [], isLoading } = useQuery({
    queryKey: ['client-contacts', client?.id],
    queryFn: () => api.getClientContacts(currentWorkspace!.id, client.id),
    enabled: !!currentWorkspace && !!client?.id && open,
  });

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createClientContact(currentWorkspace!.id, client.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['client-contacts', client?.id] });
      toast({ title: "Sucesso", description: "Contato adicionado com sucesso!" });
      resetForm();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ contactId, data }: { contactId: string; data: any }) => 
      api.updateClientContact(currentWorkspace!.id, client.id, contactId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['client-contacts', client?.id] });
      toast({ title: "Sucesso", description: "Contato atualizado com sucesso!" });
      resetForm();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (contactId: string) => api.deleteClientContact(currentWorkspace!.id, client.id, contactId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['client-contacts', client?.id] });
      toast({ title: "Sucesso", description: "Contato removido com sucesso!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const setPrimaryMutation = useMutation({
    mutationFn: (contactId: string) => api.setPrimaryContact(currentWorkspace!.id, client.id, contactId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['client-contacts', client?.id] });
      toast({ title: "Sucesso", description: "Contato principal definido!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const resetForm = () => {
    setFormData({ name: "", role: "", email: "", phone: "", whatsapp: "", isPrimary: false, notes: "" });
    setShowAddForm(false);
    setEditingContact(null);
  };

  const handleEdit = (contact: any) => {
    setFormData({
      name: contact.name || "",
      role: contact.role || "",
      email: contact.email || "",
      phone: contact.phone || "",
      whatsapp: contact.whatsapp || "",
      isPrimary: contact.isPrimary || false,
      notes: contact.notes || "",
    });
    setEditingContact(contact);
    setShowAddForm(true);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (editingContact) {
      updateMutation.mutate({ contactId: editingContact.id, data: formData });
    } else {
      createMutation.mutate(formData);
    }
  };

  const isSubmitting = createMutation.isPending || updateMutation.isPending;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[600px] max-h-[80vh]">
        <DialogHeader>
          <DialogTitle>Contatos - {client?.name}</DialogTitle>
        </DialogHeader>
        
        <ScrollArea className="h-[50vh] pr-4">
          {isLoading ? (
            <div className="space-y-3">
              {[1, 2].map(i => (
                <Skeleton key={i} className="h-20 rounded-lg" />
              ))}
            </div>
          ) : (
            <div className="space-y-4">
              {contacts.length === 0 && !showAddForm && (
                <div className="text-center py-8 text-muted-foreground">
                  <Users className="size-12 mx-auto mb-3 opacity-50" />
                  <p>Nenhum contato cadastrado</p>
                </div>
              )}

              {contacts.map((contact: any) => (
                <div
                  key={contact.id}
                  className="p-4 border rounded-lg space-y-2"
                  data-testid={`contact-${contact.id}`}
                >
                  <div className="flex items-start justify-between">
                    <div>
                      <div className="flex items-center gap-2">
                        <span className="font-medium">{contact.name}</span>
                        {contact.isPrimary && (
                          <Badge variant="secondary" className="text-xs">Principal</Badge>
                        )}
                      </div>
                      {contact.role && (
                        <span className="text-sm text-muted-foreground">{contact.role}</span>
                      )}
                    </div>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost" size="icon" className="h-8 w-8">
                          <MoreHorizontal className="size-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end">
                        <DropdownMenuItem onClick={() => handleEdit(contact)}>
                          <Pencil className="size-4 mr-2" />
                          Editar
                        </DropdownMenuItem>
                        {!contact.isPrimary && (
                          <DropdownMenuItem onClick={() => setPrimaryMutation.mutate(contact.id)}>
                            <User className="size-4 mr-2" />
                            Definir como Principal
                          </DropdownMenuItem>
                        )}
                        <DropdownMenuItem 
                          onClick={() => deleteMutation.mutate(contact.id)}
                          className="text-destructive"
                        >
                          <Trash2 className="size-4 mr-2" />
                          Remover
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </div>
                  <div className="flex flex-wrap gap-3 text-sm text-muted-foreground">
                    {contact.email && (
                      <a href={`mailto:${contact.email}`} className="flex items-center gap-1 hover:text-foreground">
                        <Mail className="size-3.5" />
                        {contact.email}
                      </a>
                    )}
                    {contact.phone && (
                      <a href={`tel:${contact.phone}`} className="flex items-center gap-1 hover:text-foreground">
                        <Phone className="size-3.5" />
                        {contact.phone}
                      </a>
                    )}
                  </div>
                </div>
              ))}

              {showAddForm && (
                <form onSubmit={handleSubmit} className="p-4 border rounded-lg space-y-4 bg-muted/50">
                  <div className="flex items-center justify-between">
                    <h4 className="font-medium">{editingContact ? "Editar Contato" : "Novo Contato"}</h4>
                    <Button type="button" variant="ghost" size="icon" onClick={resetForm}>
                      <X className="size-4" />
                    </Button>
                  </div>

                  <div className="grid grid-cols-2 gap-4">
                    <div className="space-y-2">
                      <Label htmlFor="contact-name">Nome</Label>
                      <Input
                        id="contact-name"
                        value={formData.name}
                        onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                        placeholder="Nome do contato"
                        required
                        data-testid="input-contact-name"
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="contact-role">Cargo</Label>
                      <Input
                        id="contact-role"
                        value={formData.role}
                        onChange={(e) => setFormData({ ...formData, role: e.target.value })}
                        placeholder="Ex: Diretor, Gerente"
                        data-testid="input-contact-role"
                      />
                    </div>
                  </div>

                  <div className="grid grid-cols-2 gap-4">
                    <div className="space-y-2">
                      <Label htmlFor="contact-email">E-mail</Label>
                      <Input
                        id="contact-email"
                        type="email"
                        value={formData.email}
                        onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                        placeholder="email@exemplo.com"
                        data-testid="input-contact-email"
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="contact-phone">Telefone</Label>
                      <Input
                        id="contact-phone"
                        value={formData.phone}
                        onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                        placeholder="(11) 99999-9999"
                        data-testid="input-contact-phone"
                      />
                    </div>
                  </div>

                  <div className="flex items-center gap-2">
                    <Checkbox 
                      id="contact-primary"
                      checked={formData.isPrimary}
                      onCheckedChange={(checked) => setFormData({ ...formData, isPrimary: checked as boolean })}
                    />
                    <Label htmlFor="contact-primary" className="font-normal">
                      Definir como contato principal
                    </Label>
                  </div>

                  <div className="flex justify-end gap-2">
                    <Button type="button" variant="outline" onClick={resetForm}>
                      Cancelar
                    </Button>
                    <Button type="submit" disabled={isSubmitting} data-testid="button-save-contact">
                      {isSubmitting && <Loader2 className="mr-2 size-4 animate-spin" />}
                      {editingContact ? "Salvar" : "Adicionar"}
                    </Button>
                  </div>
                </form>
              )}

              {!showAddForm && (
                <Button
                  variant="outline"
                  className="w-full gap-2"
                  onClick={() => setShowAddForm(true)}
                  data-testid="button-add-contact"
                >
                  <Plus className="size-4" />
                  Adicionar Contato
                </Button>
              )}
            </div>
          )}
        </ScrollArea>
      </DialogContent>
    </Dialog>
  );
}

export default function ClientsPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [contactsModalOpen, setContactsModalOpen] = useState(false);
  const [selectedClient, setSelectedClient] = useState<any>(null);
  
  const [page, setPage] = useState(1);
  const [searchInput, setSearchInput] = useState('');
  const [search, setSearch] = useState('');
  const [sortBy, setSortBy] = useState('newest');
  const [clientType, setClientType] = useState('');
  const pageSize = 20;

  const { data, isLoading } = useQuery({
    queryKey: ['clients-paginated', currentWorkspace?.id, page, search, sortBy, clientType],
    queryFn: async () => {
      const params = new URLSearchParams();
      params.set('page', page.toString());
      params.set('pageSize', pageSize.toString());
      if (search) params.set('search', search);
      if (sortBy) params.set('sortBy', sortBy);
      if (clientType) params.set('clientType', clientType);
      
      const response = await fetch(`/api/workspaces/${currentWorkspace!.id}/clients?${params.toString()}`, {
        credentials: 'include',
      });
      if (!response.ok) throw new Error('Failed to fetch clients');
      return response.json();
    },
    enabled: !!currentWorkspace,
  });

  const clients = data?.items || [];
  const total = data?.total || 0;
  const totalPages = Math.ceil(total / pageSize);

  useEffect(() => {
    const timer = setTimeout(() => {
      setSearch(searchInput);
      setPage(1);
    }, 300);
    return () => clearTimeout(timer);
  }, [searchInput]);

  useEffect(() => {
    setPage(1);
    setSearchInput('');
    setSearch('');
    setSortBy('newest');
    setClientType('');
  }, [currentWorkspace?.id]);

  const deleteMutation = useMutation({
    mutationFn: (clientId: string) => api.deleteClient(currentWorkspace!.id, clientId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clients-paginated'] });
      toast({ title: "Sucesso", description: "Cliente excluído com sucesso!" });
      setDeleteModalOpen(false);
      setSelectedClient(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleEdit = (client: any) => {
    setSelectedClient(client);
    setModalOpen(true);
  };

  const handleDelete = (client: any) => {
    setSelectedClient(client);
    setDeleteModalOpen(true);
  };

  const handleNewClient = () => {
    setSelectedClient(null);
    setModalOpen(true);
  };

  const handleContacts = (client: any) => {
    setSelectedClient(client);
    setContactsModalOpen(true);
  };

  const getClientDocument = (client: any) => {
    if (client.clientType === "PERSON") {
      return client.cpf ? formatCPF(client.cpf) : null;
    } else {
      return client.cnpj ? formatCNPJ(client.cnpj) : null;
    }
  };

  const getDisplayName = (client: any) => {
    if (client.clientType === "COMPANY") {
      return client.tradeName || client.companyName || client.name;
    }
    return client.name;
  };

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Clientes</h1>
            <p className="text-muted-foreground">Gerencie seus relacionamentos com clientes.</p>
          </div>
          <Button className="gap-2" onClick={handleNewClient} data-testid="button-add-client">
            <Plus className="size-4" />
            Adicionar Cliente
          </Button>
        </div>

        <div className="flex flex-col sm:flex-row gap-4 items-start sm:items-center">
          <div className="relative flex-1 max-w-sm">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground" />
            <Input
              placeholder="Buscar por nome, email, telefone..."
              value={searchInput}
              onChange={(e) => setSearchInput(e.target.value)}
              className="pl-9"
              data-testid="input-search-clients"
            />
          </div>
          
          <div className="flex gap-2 items-center">
            <Select value={clientType} onValueChange={(v) => { setClientType(v === 'all' ? '' : v); setPage(1); }}>
              <SelectTrigger className="w-[140px]" data-testid="select-client-type">
                <SelectValue placeholder="Tipo" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">Todos</SelectItem>
                <SelectItem value="PERSON">Pessoa Física</SelectItem>
                <SelectItem value="COMPANY">Pessoa Jurídica</SelectItem>
              </SelectContent>
            </Select>

            <Select value={sortBy} onValueChange={(v) => { setSortBy(v); setPage(1); }}>
              <SelectTrigger className="w-[160px]" data-testid="select-sort-clients">
                <ArrowUpDown className="size-4 mr-2" />
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="newest">Mais recentes</SelectItem>
                <SelectItem value="oldest">Mais antigos</SelectItem>
                <SelectItem value="name_asc">Nome A-Z</SelectItem>
                <SelectItem value="name_desc">Nome Z-A</SelectItem>
                <SelectItem value="updated">Última atualização</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>

        {total > 0 && (
          <p className="text-sm text-muted-foreground">
            Mostrando {((page - 1) * pageSize) + 1}-{Math.min(page * pageSize, total)} de {total} clientes
          </p>
        )}

        {isLoading ? (
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {[...Array(6)].map((_, i) => (
              <Skeleton key={i} className="h-48 rounded-xl" />
            ))}
          </div>
        ) : (
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {clients.map((client: any) => (
              <TactileCard key={client.id} hover className="p-6 flex flex-col items-center text-center space-y-4 relative group" data-testid={`card-client-${client.id}`}>
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button variant="ghost" size="icon" className="absolute top-2 right-2 opacity-0 group-hover:opacity-100 transition-opacity">
                      <MoreHorizontal className="size-4" />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                    <DropdownMenuItem onClick={() => handleEdit(client)}>
                      <Pencil className="size-4 mr-2" />
                      Editar
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={() => handleContacts(client)}>
                      <Users className="size-4 mr-2" />
                      Contatos
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={() => handleDelete(client)} className="text-destructive">
                      <Trash2 className="size-4 mr-2" />
                      Excluir
                    </DropdownMenuItem>
                  </DropdownMenuContent>
                </DropdownMenu>
                
                <div className="absolute top-2 left-2 flex items-center gap-2">
                  <span className={`inline-flex size-2.5 rounded-full ${client.status === 'ACTIVE' ? 'bg-green-500' : 'bg-gray-300'}`} />
                  <Badge 
                    variant={client.clientType === "PERSON" ? "outline" : "secondary"}
                    className="text-[10px] px-1.5 py-0"
                    data-testid={`badge-type-${client.id}`}
                  >
                    {client.clientType === "PERSON" ? (
                      <><User className="size-2.5 mr-0.5" />PF</>
                    ) : (
                      <><Building2 className="size-2.5 mr-0.5" />PJ</>
                    )}
                  </Badge>
                </div>

                <Avatar className="size-20 border-2 border-background shadow-lg">
                  <AvatarImage src={client.avatar} />
                  <AvatarFallback className="text-xl">{client.name.charAt(0)}</AvatarFallback>
                </Avatar>
                
                <div>
                  <h3 className="text-lg font-semibold">{getDisplayName(client)}</h3>
                  {client.clientType === "COMPANY" && client.tradeName && client.companyName && (
                    <div className="flex items-center justify-center gap-1.5 text-sm text-muted-foreground mt-1">
                      <Building2 className="size-3.5" />
                      <span>{client.companyName}</span>
                    </div>
                  )}
                  {getClientDocument(client) && (
                    <p className="text-xs text-muted-foreground mt-1 font-mono" data-testid={`document-${client.id}`}>
                      {getClientDocument(client)}
                    </p>
                  )}
                </div>

                <div className="flex items-center gap-2 w-full pt-2">
                  <Button variant="outline" className="flex-1 gap-2 h-9 text-xs" asChild>
                    <Link href={`/${currentWorkspace?.slug}/clients/${client.id}`}>
                      <Eye className="size-3.5" />
                      Detalhes
                    </Link>
                  </Button>
                  <Button 
                    variant="outline" 
                    className="flex-1 gap-2 h-9 text-xs" 
                    onClick={() => handleContacts(client)}
                    data-testid={`button-contacts-${client.id}`}
                  >
                    <Users className="size-3.5" />
                    Contatos
                  </Button>
                </div>
              </TactileCard>
            ))}
            
            <button 
              onClick={handleNewClient}
              className="flex flex-col items-center justify-center p-6 rounded-xl border-2 border-dashed border-muted-foreground/20 hover:border-primary/50 hover:bg-muted/50 transition-all group h-full min-h-[200px]"
              data-testid="button-add-client-card"
            >
              <div className="size-12 rounded-full bg-muted flex items-center justify-center group-hover:bg-primary/10 group-hover:text-primary transition-colors">
                <Plus className="size-6" />
              </div>
              <span className="mt-3 font-medium text-muted-foreground group-hover:text-foreground">Adicionar Novo Cliente</span>
            </button>
          </div>
        )}

        {totalPages > 1 && (
          <div className="flex items-center justify-center gap-2 pt-4">
            <Button
              variant="outline"
              size="sm"
              onClick={() => setPage(p => Math.max(1, p - 1))}
              disabled={page === 1}
              data-testid="button-prev-page"
            >
              <ChevronLeft className="size-4" />
              Anterior
            </Button>
            <span className="text-sm text-muted-foreground px-4">
              Página {page} de {totalPages}
            </span>
            <Button
              variant="outline"
              size="sm"
              onClick={() => setPage(p => Math.min(totalPages, p + 1))}
              disabled={page === totalPages}
              data-testid="button-next-page"
            >
              Próxima
              <ChevronRight className="size-4" />
            </Button>
          </div>
        )}
      </div>

      <ClientModal
        open={modalOpen}
        onOpenChange={setModalOpen}
        client={selectedClient}
      />

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteMutation.mutate(selectedClient?.id)}
        title="Excluir Cliente"
        description={`Tem certeza que deseja excluir "${selectedClient?.name}"? Esta ação não pode ser desfeita.`}
        isLoading={deleteMutation.isPending}
      />

      <ContactsModal
        open={contactsModalOpen}
        onOpenChange={setContactsModalOpen}
        client={selectedClient}
      />
    </AppShell>
  );
}
