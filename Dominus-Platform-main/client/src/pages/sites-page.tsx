import { useState, useEffect } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { useToast } from "@/hooks/use-toast";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
  DialogDescription,
} from "@/components/ui/dialog";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { ScrollArea } from "@/components/ui/scroll-area";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
  DropdownMenuSeparator,
} from "@/components/ui/dropdown-menu";
import { DeleteModal } from "@/components/modals/delete-modal";
import { 
  Plus, 
  MoreVertical, 
  Pencil, 
  Trash2, 
  Globe,
  Eye,
  Settings,
  FileText,
  Layers,
  ExternalLink,
  BookOpen,
} from "lucide-react";
import { Link } from "wouter";

const statusLabels: Record<string, string> = {
  DRAFT: 'Rascunho',
  PUBLISHED: 'Publicado',
  ARCHIVED: 'Arquivado',
};

const statusColors: Record<string, string> = {
  DRAFT: 'bg-yellow-100 text-yellow-800',
  PUBLISHED: 'bg-green-100 text-green-800',
  ARCHIVED: 'bg-gray-100 text-gray-800',
};

interface SiteModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  site?: any;
  onSuccess: () => void;
}

function SiteModal({ open, onOpenChange, site, onSuccess }: SiteModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  
  const [formData, setFormData] = useState({
    name: '',
    slug: '',
    description: '',
  });

  useEffect(() => {
    if (open) {
      setFormData({
        name: site?.name || '',
        slug: site?.slug || '',
        description: site?.description || '',
      });
    }
  }, [open, site]);

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createSiteProject(currentWorkspace!.id, data),
    onSuccess: () => {
      toast({ title: "Sucesso", description: "Site criado com sucesso!" });
      onSuccess();
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: (data: any) => api.updateSiteProject(currentWorkspace!.id, site.id, data),
    onSuccess: () => {
      toast({ title: "Sucesso", description: "Site atualizado com sucesso!" });
      onSuccess();
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (site) {
      updateMutation.mutate(formData);
    } else {
      createMutation.mutate(formData);
    }
  };

  const isLoading = createMutation.isPending || updateMutation.isPending;

  const generateSlug = (name: string) => {
    return name
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/(^-|-$)/g, '');
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-md">
        <DialogHeader>
          <DialogTitle>{site ? 'Editar Site' : 'Novo Site'}</DialogTitle>
          <DialogDescription>
            {site ? 'Edite as informações básicas do site' : 'Crie um novo site para começar a construir páginas'}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="name">Nome do Site</Label>
            <Input
              id="name"
              value={formData.name}
              onChange={(e) => {
                const name = e.target.value;
                setFormData({ 
                  ...formData, 
                  name,
                  slug: site ? formData.slug : generateSlug(name)
                });
              }}
              placeholder="Meu Site Incrível"
              data-testid="input-site-name"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="slug">URL do Site</Label>
            <div className="flex items-center gap-2">
              <span className="text-sm text-muted-foreground">/site/</span>
              <Input
                id="slug"
                value={formData.slug}
                onChange={(e) => setFormData({ ...formData, slug: e.target.value })}
                placeholder="meu-site"
                data-testid="input-site-slug"
              />
            </div>
          </div>
          <div className="space-y-2">
            <Label htmlFor="description">Descrição</Label>
            <Textarea
              id="description"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              placeholder="Uma breve descrição do seu site..."
              rows={3}
              data-testid="input-site-description"
            />
          </div>
          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={isLoading || !formData.name.trim()} data-testid="button-save-site">
              {isLoading ? 'Salvando...' : site ? 'Salvar' : 'Criar Site'}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}

interface SiteSettingsModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  site: any;
  onSuccess: () => void;
}

function SiteSettingsModal({ open, onOpenChange, site, onSuccess }: SiteSettingsModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  
  const [formData, setFormData] = useState({
    seoTitle: '',
    seoDescription: '',
    seoImage: '',
    logoUrl: '',
    faviconUrl: '',
    primaryColor: '#3B82F6',
    secondaryColor: '#10B981',
    fontFamily: 'Inter',
    headScripts: '',
    bodyStartScripts: '',
    bodyEndScripts: '',
  });

  useEffect(() => {
    if (open && site) {
      setFormData({
        seoTitle: site.seoTitle || '',
        seoDescription: site.seoDescription || '',
        seoImage: site.seoImage || '',
        logoUrl: site.logoUrl || '',
        faviconUrl: site.faviconUrl || '',
        primaryColor: site.primaryColor || '#3B82F6',
        secondaryColor: site.secondaryColor || '#10B981',
        fontFamily: site.fontFamily || 'Inter',
        headScripts: site.headScripts || '',
        bodyStartScripts: site.bodyStartScripts || '',
        bodyEndScripts: site.bodyEndScripts || '',
      });
    }
  }, [open, site]);

  const updateMutation = useMutation({
    mutationFn: (data: any) => api.updateSiteProject(currentWorkspace!.id, site.id, data),
    onSuccess: () => {
      toast({ title: "Sucesso", description: "Configurações salvas com sucesso!" });
      onSuccess();
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    updateMutation.mutate(formData);
  };

  if (!site) return null;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-2xl max-h-[90vh]">
        <DialogHeader>
          <DialogTitle>Configurações do Site</DialogTitle>
          <DialogDescription>
            Configure SEO, branding e scripts do seu site
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit}>
          <ScrollArea className="h-[60vh] pr-4">
            <Tabs defaultValue="seo" className="w-full">
              <TabsList className="w-full mb-4">
                <TabsTrigger value="seo" className="flex-1">SEO</TabsTrigger>
                <TabsTrigger value="branding" className="flex-1">Branding</TabsTrigger>
                <TabsTrigger value="scripts" className="flex-1">Scripts</TabsTrigger>
              </TabsList>

              <TabsContent value="seo" className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="seoTitle">Título SEO</Label>
                  <Input
                    id="seoTitle"
                    value={formData.seoTitle}
                    onChange={(e) => setFormData({ ...formData, seoTitle: e.target.value })}
                    placeholder="Título para mecanismos de busca"
                  />
                  <p className="text-xs text-muted-foreground">Aparece nos resultados do Google</p>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="seoDescription">Descrição SEO</Label>
                  <Textarea
                    id="seoDescription"
                    value={formData.seoDescription}
                    onChange={(e) => setFormData({ ...formData, seoDescription: e.target.value })}
                    placeholder="Descrição para mecanismos de busca"
                    rows={3}
                  />
                  <p className="text-xs text-muted-foreground">Descrição exibida nos resultados de busca (máx. 160 caracteres)</p>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="seoImage">Imagem de Compartilhamento (OG Image)</Label>
                  <Input
                    id="seoImage"
                    value={formData.seoImage}
                    onChange={(e) => setFormData({ ...formData, seoImage: e.target.value })}
                    placeholder="https://exemplo.com/imagem.jpg"
                  />
                  <p className="text-xs text-muted-foreground">Imagem exibida ao compartilhar nas redes sociais (1200x630px recomendado)</p>
                </div>
              </TabsContent>

              <TabsContent value="branding" className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="logoUrl">URL do Logo</Label>
                  <Input
                    id="logoUrl"
                    value={formData.logoUrl}
                    onChange={(e) => setFormData({ ...formData, logoUrl: e.target.value })}
                    placeholder="https://exemplo.com/logo.png"
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="faviconUrl">URL do Favicon</Label>
                  <Input
                    id="faviconUrl"
                    value={formData.faviconUrl}
                    onChange={(e) => setFormData({ ...formData, faviconUrl: e.target.value })}
                    placeholder="https://exemplo.com/favicon.ico"
                  />
                  <p className="text-xs text-muted-foreground">Ícone exibido na aba do navegador</p>
                </div>
                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="primaryColor">Cor Primária</Label>
                    <div className="flex gap-2">
                      <input
                        type="color"
                        id="primaryColor"
                        value={formData.primaryColor}
                        onChange={(e) => setFormData({ ...formData, primaryColor: e.target.value })}
                        className="w-10 h-10 rounded cursor-pointer"
                      />
                      <Input
                        value={formData.primaryColor}
                        onChange={(e) => setFormData({ ...formData, primaryColor: e.target.value })}
                        className="flex-1"
                      />
                    </div>
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="secondaryColor">Cor Secundária</Label>
                    <div className="flex gap-2">
                      <input
                        type="color"
                        id="secondaryColor"
                        value={formData.secondaryColor}
                        onChange={(e) => setFormData({ ...formData, secondaryColor: e.target.value })}
                        className="w-10 h-10 rounded cursor-pointer"
                      />
                      <Input
                        value={formData.secondaryColor}
                        onChange={(e) => setFormData({ ...formData, secondaryColor: e.target.value })}
                        className="flex-1"
                      />
                    </div>
                  </div>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="fontFamily">Fonte</Label>
                  <Input
                    id="fontFamily"
                    value={formData.fontFamily}
                    onChange={(e) => setFormData({ ...formData, fontFamily: e.target.value })}
                    placeholder="Inter, system-ui, sans-serif"
                  />
                </div>
              </TabsContent>

              <TabsContent value="scripts" className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="headScripts">Scripts no Head</Label>
                  <Textarea
                    id="headScripts"
                    value={formData.headScripts}
                    onChange={(e) => setFormData({ ...formData, headScripts: e.target.value })}
                    placeholder="<script>...</script> ou <link>...</link>"
                    rows={4}
                    className="font-mono text-sm"
                  />
                  <p className="text-xs text-muted-foreground">Códigos inseridos antes de fechar a tag head (ex: Google Analytics, Meta Pixel)</p>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="bodyStartScripts">Scripts no Início do Body</Label>
                  <Textarea
                    id="bodyStartScripts"
                    value={formData.bodyStartScripts}
                    onChange={(e) => setFormData({ ...formData, bodyStartScripts: e.target.value })}
                    placeholder="<script>...</script>"
                    rows={3}
                    className="font-mono text-sm"
                  />
                  <p className="text-xs text-muted-foreground">Códigos inseridos logo após abrir a tag body</p>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="bodyEndScripts">Scripts no Final do Body</Label>
                  <Textarea
                    id="bodyEndScripts"
                    value={formData.bodyEndScripts}
                    onChange={(e) => setFormData({ ...formData, bodyEndScripts: e.target.value })}
                    placeholder="<script>...</script>"
                    rows={3}
                    className="font-mono text-sm"
                  />
                  <p className="text-xs text-muted-foreground">Códigos inseridos antes de fechar a tag body</p>
                </div>
              </TabsContent>
            </Tabs>
          </ScrollArea>
          <DialogFooter className="mt-4">
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={updateMutation.isPending}>
              {updateMutation.isPending ? 'Salvando...' : 'Salvar Configurações'}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}

export default function SitesPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  
  const [siteModalOpen, setSiteModalOpen] = useState(false);
  const [editingSite, setEditingSite] = useState<any>(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [siteToDelete, setSiteToDelete] = useState<any>(null);
  const [settingsModalOpen, setSettingsModalOpen] = useState(false);
  const [settingsSite, setSettingsSite] = useState<any>(null);

  const { data: sites = [], isLoading } = useQuery({
    queryKey: ['sites', currentWorkspace?.id],
    queryFn: () => api.getSiteProjects(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const deleteMutation = useMutation({
    mutationFn: (siteId: string) => api.deleteSiteProject(currentWorkspace!.id, siteId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['sites'] });
      toast({ title: "Sucesso", description: "Site excluído com sucesso!" });
      setDeleteModalOpen(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleRefresh = () => {
    queryClient.invalidateQueries({ queryKey: ['sites'] });
  };

  if (!currentWorkspace) return null;

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-bold">Sites</h1>
            <p className="text-muted-foreground">Crie e gerencie seus sites com o construtor visual</p>
          </div>
          <Button onClick={() => { setEditingSite(null); setSiteModalOpen(true); }} data-testid="button-new-site">
            <Plus className="size-4 mr-2" />
            Novo Site
          </Button>
        </div>

        {isLoading ? (
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {[1, 2, 3].map((i) => (
              <Skeleton key={i} className="h-64 rounded-xl" />
            ))}
          </div>
        ) : sites.length === 0 ? (
          <TactileCard className="p-12 text-center">
            <div className="flex flex-col items-center gap-4">
              <div className="size-16 rounded-full bg-primary/10 flex items-center justify-center">
                <Globe className="size-8 text-primary" />
              </div>
              <div>
                <h3 className="font-semibold text-lg mb-1">Nenhum site ainda</h3>
                <p className="text-muted-foreground mb-4">
                  Crie seu primeiro site e comece a construir páginas incríveis
                </p>
                <Button onClick={() => { setEditingSite(null); setSiteModalOpen(true); }}>
                  <Plus className="size-4 mr-2" />
                  Criar Primeiro Site
                </Button>
              </div>
            </div>
          </TactileCard>
        ) : (
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {sites.map((site: any) => (
              <TactileCard key={site.id} className="overflow-hidden" data-testid={`card-site-${site.id}`}>
                <div className="aspect-video bg-gradient-to-br from-primary/20 to-primary/5 flex items-center justify-center">
                  <Globe className="size-12 text-primary/50" />
                </div>
                <div className="p-4 space-y-4">
                  <div className="flex items-start justify-between">
                    <div className="flex-1 min-w-0">
                      <h3 className="font-semibold truncate">{site.name}</h3>
                      <p className="text-sm text-muted-foreground truncate">/{site.slug}</p>
                    </div>
                    <Badge className={statusColors[site.status] || 'bg-gray-100'}>
                      {statusLabels[site.status] || site.status}
                    </Badge>
                  </div>
                  
                  {site.description && (
                    <p className="text-sm text-muted-foreground line-clamp-2">{site.description}</p>
                  )}

                  <div className="flex items-center gap-2 pt-2 border-t">
                    <Link href={`/${currentWorkspace.slug}/sites/${site.id}/builder`}>
                      <Button size="sm" className="flex-1" data-testid={`button-edit-site-${site.id}`}>
                        <Layers className="size-4 mr-2" />
                        Editor
                      </Button>
                    </Link>
                    <Button 
                      size="sm" 
                      variant="outline"
                      onClick={() => window.open(`/site/${currentWorkspace.slug}/${site.slug}?preview=true`, '_blank')}
                      title="Visualizar Site"
                    >
                      <Eye className="size-4" />
                    </Button>
                    <Link href={`/${currentWorkspace.slug}/sites/${site.id}/blog`}>
                      <Button size="sm" variant="outline" title="Blog">
                        <BookOpen className="size-4" />
                      </Button>
                    </Link>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button size="sm" variant="ghost">
                          <MoreVertical className="size-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end">
                        <DropdownMenuItem onClick={() => { setEditingSite(site); setSiteModalOpen(true); }}>
                          <Pencil className="size-4 mr-2" />
                          Editar
                        </DropdownMenuItem>
                        <DropdownMenuItem onClick={() => window.open(`/site/${currentWorkspace.slug}/${site.slug}`, '_blank')}>
                          <ExternalLink className="size-4 mr-2" />
                          Ver Site
                        </DropdownMenuItem>
                        <DropdownMenuItem onClick={() => { setSettingsSite(site); setSettingsModalOpen(true); }}>
                          <Settings className="size-4 mr-2" />
                          Configurações
                        </DropdownMenuItem>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem 
                          className="text-red-600"
                          onClick={() => { setSiteToDelete(site); setDeleteModalOpen(true); }}
                        >
                          <Trash2 className="size-4 mr-2" />
                          Excluir
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </div>
                </div>
              </TactileCard>
            ))}
          </div>
        )}
      </div>

      <SiteModal
        open={siteModalOpen}
        onOpenChange={setSiteModalOpen}
        site={editingSite}
        onSuccess={handleRefresh}
      />

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteMutation.mutate(siteToDelete?.id)}
        title="Excluir Site"
        description={`Tem certeza que deseja excluir o site "${siteToDelete?.name}"? Todas as páginas e posts serão excluídos. Esta ação não pode ser desfeita.`}
        isLoading={deleteMutation.isPending}
      />

      <SiteSettingsModal
        open={settingsModalOpen}
        onOpenChange={setSettingsModalOpen}
        site={settingsSite}
        onSuccess={handleRefresh}
      />
    </AppShell>
  );
}
