import { useState, useCallback, useEffect } from "react";
import { useParams } from "wouter";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Skeleton } from "@/components/ui/skeleton";
import { useToast } from "@/hooks/use-toast";
import { 
  BLOCK_REGISTRY, 
  BlockDefinition, 
  getAllCategories,
  getBlocksByCategory,
  getBlockByKey,
} from "@/site-builder/blocks";
import { BlockRenderer } from "@/site-builder/blocks/renderers";
import { BlockContent, BLOCK_CATEGORY_LABELS } from "@shared/schema";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
  DialogDescription,
} from "@/components/ui/dialog";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Switch } from "@/components/ui/switch";
import {
  DndContext,
  DragEndEvent,
  DragOverlay,
  DragStartEvent,
  PointerSensor,
  useSensor,
  useSensors,
  closestCenter,
} from "@dnd-kit/core";
import {
  SortableContext,
  useSortable,
  verticalListSortingStrategy,
  arrayMove,
} from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { 
  ArrowLeft, 
  Plus, 
  Save, 
  Eye, 
  Settings, 
  Trash2, 
  GripVertical,
  Layers,
  FileText,
  Globe,
  ChevronRight,
} from "lucide-react";
import { Link } from "wouter";
import { cn } from "@/lib/utils";

function generateBlockId() {
  return `block-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
}

interface SortableBlockProps {
  block: BlockContent;
  isSelected: boolean;
  onSelect: () => void;
  onDelete: () => void;
}

function SortableBlock({ block, isSelected, onSelect, onDelete }: SortableBlockProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: block.id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  const definition = getBlockByKey(block.type);

  return (
    <div
      ref={setNodeRef}
      style={style}
      className={cn(
        "relative group border-2 rounded-lg transition-colors",
        isSelected ? "border-primary" : "border-transparent hover:border-primary/50"
      )}
    >
      <div 
        className="absolute top-2 left-2 z-10 flex items-center gap-1 bg-background/90 backdrop-blur rounded-md shadow-sm border px-2 py-1 opacity-0 group-hover:opacity-100 transition-opacity"
      >
        <button
          {...attributes}
          {...listeners}
          className="cursor-grab hover:text-primary"
        >
          <GripVertical className="size-4" />
        </button>
        <span className="text-xs font-medium">{definition?.label || block.type}</span>
        <button
          onClick={onDelete}
          className="text-red-500 hover:text-red-600 ml-2"
        >
          <Trash2 className="size-3" />
        </button>
      </div>
      <div onClick={onSelect} className="cursor-pointer">
        <BlockRenderer block={block} isPreview />
      </div>
    </div>
  );
}

interface BlockInspectorProps {
  block: BlockContent | null;
  onChange: (props: Record<string, any>) => void;
  forms: any[];
}

function BlockInspector({ block, onChange, forms }: BlockInspectorProps) {
  if (!block) {
    return (
      <div className="p-4 text-center text-muted-foreground">
        <Layers className="size-12 mx-auto mb-4 opacity-50" />
        <p>Selecione um bloco para editar suas propriedades</p>
      </div>
    );
  }

  const definition = getBlockByKey(block.type);
  if (!definition) return null;

  return (
    <div className="p-4 space-y-4">
      <div>
        <h3 className="font-semibold mb-1">{definition.label}</h3>
        <p className="text-sm text-muted-foreground">{definition.description}</p>
      </div>
      <div className="space-y-4">
        {definition.schema.map((prop) => {
          const value = block.props[prop.key] ?? definition.defaultProps[prop.key];

          switch (prop.type) {
            case 'text':
              return (
                <div key={prop.key} className="space-y-2">
                  <Label>{prop.label}</Label>
                  <Input
                    value={value || ''}
                    onChange={(e) => onChange({ ...block.props, [prop.key]: e.target.value })}
                  />
                </div>
              );
            case 'textarea':
              return (
                <div key={prop.key} className="space-y-2">
                  <Label>{prop.label}</Label>
                  <Textarea
                    value={value || ''}
                    onChange={(e) => onChange({ ...block.props, [prop.key]: e.target.value })}
                    rows={3}
                  />
                </div>
              );
            case 'number':
              return (
                <div key={prop.key} className="space-y-2">
                  <Label>{prop.label}</Label>
                  <Input
                    type="number"
                    value={value || ''}
                    onChange={(e) => onChange({ ...block.props, [prop.key]: Number(e.target.value) })}
                  />
                </div>
              );
            case 'boolean':
              return (
                <div key={prop.key} className="flex items-center justify-between">
                  <Label>{prop.label}</Label>
                  <Switch
                    checked={!!value}
                    onCheckedChange={(checked) => onChange({ ...block.props, [prop.key]: checked })}
                  />
                </div>
              );
            case 'color':
              return (
                <div key={prop.key} className="space-y-2">
                  <Label>{prop.label}</Label>
                  <div className="flex gap-2">
                    <input
                      type="color"
                      value={value || '#000000'}
                      onChange={(e) => onChange({ ...block.props, [prop.key]: e.target.value })}
                      className="w-12 h-10 rounded border cursor-pointer"
                    />
                    <Input
                      value={value || ''}
                      onChange={(e) => onChange({ ...block.props, [prop.key]: e.target.value })}
                      placeholder="#000000"
                    />
                  </div>
                </div>
              );
            case 'select':
              return (
                <div key={prop.key} className="space-y-2">
                  <Label>{prop.label}</Label>
                  <Select
                    value={String(value)}
                    onValueChange={(val) => onChange({ ...block.props, [prop.key]: val })}
                  >
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      {prop.options?.map((opt) => (
                        <SelectItem key={opt.value} value={opt.value}>
                          {opt.label}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
              );
            case 'image':
              return (
                <div key={prop.key} className="space-y-2">
                  <Label>{prop.label}</Label>
                  <Input
                    value={value || ''}
                    onChange={(e) => onChange({ ...block.props, [prop.key]: e.target.value })}
                    placeholder="https://..."
                  />
                  {value && (
                    <img src={value} alt="" className="w-full h-20 object-cover rounded" />
                  )}
                </div>
              );
            case 'form-select':
              return (
                <div key={prop.key} className="space-y-2">
                  <Label>{prop.label}</Label>
                  <Select
                    value={value || ''}
                    onValueChange={(val) => onChange({ ...block.props, [prop.key]: val })}
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Selecione um formulário" />
                    </SelectTrigger>
                    <SelectContent>
                      {forms.map((form: any) => (
                        <SelectItem key={form.id} value={form.id}>
                          {form.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
              );
            default:
              return null;
          }
        })}
      </div>
    </div>
  );
}

export default function SiteBuilderPage() {
  const { slug, siteId } = useParams();
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [blocks, setBlocks] = useState<BlockContent[]>([]);
  const [selectedBlockId, setSelectedBlockId] = useState<string | null>(null);
  const [activeCategory, setActiveCategory] = useState<string>('HERO');
  const [hasChanges, setHasChanges] = useState(false);
  const [addBlockOpen, setAddBlockOpen] = useState(false);
  const [activeId, setActiveId] = useState<string | null>(null);
  const [homePageId, setHomePageId] = useState<string | null>(null);

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8,
      },
    })
  );

  const { data: site, isLoading: siteLoading } = useQuery({
    queryKey: ['site', siteId],
    queryFn: () => api.getSiteProject(currentWorkspace!.id, siteId!),
    enabled: !!currentWorkspace && !!siteId,
  });

  const { data: pages = [], isLoading: pagesLoading } = useQuery({
    queryKey: ['site-pages', siteId],
    queryFn: () => api.getSitePages(siteId!),
    enabled: !!currentWorkspace && !!siteId,
  });

  const { data: forms = [] } = useQuery({
    queryKey: ['lead-forms', currentWorkspace?.id],
    queryFn: () => api.getLeadForms(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const homePage = pages.find((p: any) => p.isHomePage);
  const effectiveHomePageId = homePageId || homePage?.id;

  const { data: currentPage, isLoading: pageLoading } = useQuery({
    queryKey: ['site-page', effectiveHomePageId],
    queryFn: () => api.getSitePage(siteId!, effectiveHomePageId!),
    enabled: !!effectiveHomePageId && !!siteId,
  });

  useEffect(() => {
    if (currentPage) {
      try {
        const content = JSON.parse(currentPage.content || '[]');
        setBlocks(content);
      } catch {
        setBlocks([]);
      }
    }
  }, [currentPage]);

  const saveMutation = useMutation({
    mutationFn: async () => {
      if (effectiveHomePageId) {
        return api.updateSitePage(siteId!, effectiveHomePageId, {
          content: JSON.stringify(blocks),
        });
      } else {
        const newPage = await api.createSitePage(siteId!, {
          title: 'Página Inicial',
          slug: 'home',
          isHomePage: true,
          content: JSON.stringify(blocks),
        });
        return newPage;
      }
    },
    onSuccess: (data) => {
      toast({ title: "Salvo!", description: "Alterações salvas com sucesso." });
      setHasChanges(false);
      if (data?.id && !effectiveHomePageId) {
        setHomePageId(data.id);
      }
      queryClient.invalidateQueries({ queryKey: ['site-pages', siteId] });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleDragStart = (event: DragStartEvent) => {
    setActiveId(String(event.active.id));
  };

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;
    setActiveId(null);

    if (over && active.id !== over.id) {
      const oldIndex = blocks.findIndex((b) => b.id === active.id);
      const newIndex = blocks.findIndex((b) => b.id === over.id);
      
      setBlocks(arrayMove(blocks, oldIndex, newIndex));
      setHasChanges(true);
    }
  };

  const addBlock = (definition: BlockDefinition) => {
    const newBlock: BlockContent = {
      id: generateBlockId(),
      type: definition.key,
      props: { ...definition.defaultProps },
    };
    setBlocks([...blocks, newBlock]);
    setSelectedBlockId(newBlock.id);
    setHasChanges(true);
    setAddBlockOpen(false);
  };

  const updateBlockProps = (props: Record<string, any>) => {
    setBlocks(blocks.map((b) => 
      b.id === selectedBlockId ? { ...b, props } : b
    ));
    setHasChanges(true);
  };

  const deleteBlock = (id: string) => {
    setBlocks(blocks.filter((b) => b.id !== id));
    if (selectedBlockId === id) {
      setSelectedBlockId(null);
    }
    setHasChanges(true);
  };

  const selectedBlock = blocks.find((b) => b.id === selectedBlockId);
  const categories = getAllCategories();

  if (!currentWorkspace || siteLoading || pagesLoading) {
    return (
      <div className="h-screen flex items-center justify-center">
        <Skeleton className="w-48 h-8" />
      </div>
    );
  }

  return (
    <div className="h-screen flex flex-col bg-muted/30">
      <header className="h-14 border-b bg-background flex items-center justify-between px-4">
        <div className="flex items-center gap-4">
          <Link href={`/${slug}/sites`}>
            <Button variant="ghost" size="sm" data-testid="button-back-sites">
              <ArrowLeft className="size-4 mr-2" />
              Voltar
            </Button>
          </Link>
          <div className="flex items-center gap-2 text-sm">
            <Globe className="size-4 text-muted-foreground" />
            <span className="font-medium">{site?.name}</span>
            <ChevronRight className="size-4 text-muted-foreground" />
            <span className="text-muted-foreground">Editor</span>
          </div>
        </div>
        <div className="flex items-center gap-3">
          <div className="flex items-center gap-2">
            <span className={`text-xs font-medium ${site?.isPublished ? 'text-green-600' : 'text-amber-600'}`}>
              {site?.isPublished ? 'Publicado' : 'Rascunho'}
            </span>
            <Switch
              checked={site?.isPublished || false}
              onCheckedChange={async (checked) => {
                queryClient.setQueryData(['site', siteId], (old: any) => ({
                  ...old,
                  isPublished: checked,
                }));
                try {
                  await api.updateSiteProject(currentWorkspace!.id, siteId!, { isPublished: checked });
                  queryClient.invalidateQueries({ queryKey: ['sites'] });
                  toast({ 
                    title: checked ? "Site Publicado!" : "Site em Rascunho",
                    description: checked ? "Seu site agora está visível ao público." : "Seu site agora está oculto do público."
                  });
                } catch (error: any) {
                  queryClient.setQueryData(['site', siteId], (old: any) => ({
                    ...old,
                    isPublished: !checked,
                  }));
                  toast({ title: "Erro", description: error.message, variant: "destructive" });
                }
              }}
            />
          </div>
          <div className="h-6 w-px bg-border" />
          <Link href={`/${slug}/sites/${siteId}/blog`}>
            <Button variant="outline" size="sm" data-testid="button-blog-posts">
              <FileText className="size-4 mr-2" />
              Blog
            </Button>
          </Link>
          <Button 
            variant="outline" 
            size="sm"
            onClick={() => window.open(`/site/${currentWorkspace?.slug}/${site?.slug}`, '_blank')}
            data-testid="button-preview-site"
          >
            <Eye className="size-4 mr-2" />
            Visualizar
          </Button>
          <Button 
            size="sm" 
            onClick={() => saveMutation.mutate()}
            disabled={!hasChanges || saveMutation.isPending}
            data-testid="button-save-page"
          >
            <Save className="size-4 mr-2" />
            {saveMutation.isPending ? 'Salvando...' : 'Salvar'}
          </Button>
        </div>
      </header>

      <div className="flex-1 flex overflow-hidden">
        <aside className="w-64 border-r bg-background flex flex-col">
          <div className="p-4 border-b">
            <Button className="w-full" onClick={() => setAddBlockOpen(true)} data-testid="button-add-block">
              <Plus className="size-4 mr-2" />
              Adicionar Bloco
            </Button>
          </div>
          <ScrollArea className="flex-1">
            <div className="p-4 space-y-2">
              <h3 className="text-xs font-semibold text-muted-foreground uppercase mb-2">
                Blocos na Página
              </h3>
              {blocks.length === 0 ? (
                <p className="text-sm text-muted-foreground text-center py-4">
                  Nenhum bloco adicionado
                </p>
              ) : (
                blocks.map((block) => {
                  const def = getBlockByKey(block.type);
                  return (
                    <button
                      key={block.id}
                      onClick={() => setSelectedBlockId(block.id)}
                      className={cn(
                        "w-full text-left px-3 py-2 rounded-lg text-sm flex items-center gap-2 transition-colors",
                        selectedBlockId === block.id
                          ? "bg-primary text-primary-foreground"
                          : "hover:bg-muted"
                      )}
                    >
                      {def && <def.icon className="size-4" />}
                      <span className="truncate">{def?.label || block.type}</span>
                    </button>
                  );
                })
              )}
            </div>
          </ScrollArea>
        </aside>

        <main className="flex-1 overflow-auto p-6">
          <div className="max-w-4xl mx-auto bg-background rounded-xl shadow-sm min-h-full">
            {blocks.length === 0 ? (
              <div className="h-96 flex flex-col items-center justify-center text-muted-foreground">
                <Layers className="size-16 mb-4 opacity-50" />
                <p className="text-lg font-medium mb-2">Página vazia</p>
                <p className="text-sm mb-4">Adicione blocos para começar a construir</p>
                <Button onClick={() => setAddBlockOpen(true)} data-testid="button-add-first-block">
                  <Plus className="size-4 mr-2" />
                  Adicionar Primeiro Bloco
                </Button>
              </div>
            ) : (
              <DndContext
                sensors={sensors}
                collisionDetection={closestCenter}
                onDragStart={handleDragStart}
                onDragEnd={handleDragEnd}
              >
                <SortableContext
                  items={blocks.map((b) => b.id)}
                  strategy={verticalListSortingStrategy}
                >
                  <div className="space-y-0">
                    {blocks.map((block) => (
                      <SortableBlock
                        key={block.id}
                        block={block}
                        isSelected={selectedBlockId === block.id}
                        onSelect={() => setSelectedBlockId(block.id)}
                        onDelete={() => deleteBlock(block.id)}
                      />
                    ))}
                  </div>
                </SortableContext>
                <DragOverlay>
                  {activeId && (
                    <div className="bg-muted rounded-lg p-4 shadow-lg opacity-80">
                      {getBlockByKey(blocks.find((b) => b.id === activeId)?.type || '')?.label}
                    </div>
                  )}
                </DragOverlay>
              </DndContext>
            )}
          </div>
        </main>

        <aside className="w-80 border-l bg-background flex flex-col">
          <div className="p-4 border-b">
            <h3 className="font-semibold flex items-center gap-2">
              <Settings className="size-4" />
              Propriedades
            </h3>
          </div>
          <ScrollArea className="flex-1">
            <BlockInspector 
              block={selectedBlock || null} 
              onChange={updateBlockProps}
              forms={forms}
            />
          </ScrollArea>
        </aside>
      </div>

      <Dialog open={addBlockOpen} onOpenChange={setAddBlockOpen}>
        <DialogContent className="max-w-2xl max-h-[80vh] flex flex-col">
          <DialogHeader>
            <DialogTitle>Adicionar Bloco</DialogTitle>
            <DialogDescription>
              Escolha um bloco para adicionar à sua página
            </DialogDescription>
          </DialogHeader>
          <div className="flex gap-4 flex-1 overflow-hidden">
            <div className="w-40 border-r pr-4 space-y-1">
              {categories.map((cat) => (
                <button
                  key={cat}
                  onClick={() => setActiveCategory(cat)}
                  className={cn(
                    "w-full text-left px-3 py-2 rounded-lg text-sm transition-colors",
                    activeCategory === cat
                      ? "bg-primary text-primary-foreground"
                      : "hover:bg-muted"
                  )}
                >
                  {BLOCK_CATEGORY_LABELS[cat] || cat}
                </button>
              ))}
            </div>
            <ScrollArea className="flex-1">
              <div className="grid grid-cols-2 gap-3 pr-4">
                {getBlocksByCategory(activeCategory).map((block) => (
                  <button
                    key={block.key}
                    onClick={() => addBlock(block)}
                    className="text-left p-4 border rounded-lg hover:border-primary hover:bg-primary/5 transition-colors"
                  >
                    <block.icon className="size-6 text-primary mb-2" />
                    <h4 className="font-medium text-sm">{block.label}</h4>
                    <p className="text-xs text-muted-foreground mt-1">{block.description}</p>
                  </button>
                ))}
              </div>
            </ScrollArea>
          </div>
        </DialogContent>
      </Dialog>
    </div>
  );
}
