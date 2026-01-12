import { useState, useEffect, useCallback } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams, useLocation } from "wouter";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth-store";
import { useToast } from "@/hooks/use-toast";
import { Button } from "@/components/ui/button";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Skeleton } from "@/components/ui/skeleton";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { 
  Select, 
  SelectContent, 
  SelectItem, 
  SelectTrigger, 
  SelectValue 
} from "@/components/ui/select";
import { Switch } from "@/components/ui/switch";
import { cn } from "@/lib/utils";
import {
  ArrowLeft,
  Save,
  Eye,
  Loader2,
  Plus,
  GripVertical,
  Trash2,
  Type,
  Image as ImageIcon,
  LayoutGrid,
  FileText,
  Star,
  Quote,
  DollarSign,
  Columns,
  Minus,
  Award,
  Users,
  FileCheck,
  PenTool,
  AlertCircle,
  Palette,
  Settings,
  ExternalLink,
} from "lucide-react";
import {
  DndContext,
  closestCenter,
  KeyboardSensor,
  PointerSensor,
  useSensor,
  useSensors,
  DragEndEvent,
} from "@dnd-kit/core";
import {
  arrayMove,
  SortableContext,
  sortableKeyboardCoordinates,
  useSortable,
  verticalListSortingStrategy,
} from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";

type ProposalBlockType = 
  | 'HERO' 
  | 'ABOUT' 
  | 'PORTFOLIO' 
  | 'TESTIMONIAL' 
  | 'NEED' 
  | 'SERVICES_TABLE' 
  | 'INVESTMENT' 
  | 'TERMS' 
  | 'SIGNATURE' 
  | 'TEXT' 
  | 'IMAGE' 
  | 'TWO_COLUMNS' 
  | 'DIVIDER';

interface Block {
  id: string;
  proposalId: string;
  blockType: ProposalBlockType;
  position: number;
  content: string;
  style: string;
  isVisible: boolean;
}

interface BlockContent {
  [key: string]: any;
}

interface TemplateStyle {
  primaryColor?: string;
  secondaryColor?: string;
  accentColor?: string;
  fontFamily?: string;
  headingFont?: string;
  backgroundColor?: string;
  textAlign?: string;
  dividerStyle?: string;
  margin?: string;
}

const BLOCK_TYPES: { type: ProposalBlockType; label: string; icon: any; description: string }[] = [
  { type: 'HERO', label: 'Capa', icon: Award, description: 'Título e apresentação inicial' },
  { type: 'ABOUT', label: 'Sobre Nós', icon: Users, description: 'Descrição da empresa' },
  { type: 'NEED', label: 'Necessidade', icon: AlertCircle, description: 'Problema a ser resolvido' },
  { type: 'SERVICES_TABLE', label: 'Serviços', icon: LayoutGrid, description: 'Tabela de serviços e preços' },
  { type: 'INVESTMENT', label: 'Investimento', icon: DollarSign, description: 'Total do investimento' },
  { type: 'PORTFOLIO', label: 'Portfólio', icon: ImageIcon, description: 'Trabalhos anteriores' },
  { type: 'TESTIMONIAL', label: 'Depoimento', icon: Quote, description: 'Citação de cliente' },
  { type: 'TERMS', label: 'Termos', icon: FileCheck, description: 'Termos e condições' },
  { type: 'SIGNATURE', label: 'Assinatura', icon: PenTool, description: 'Área de assinatura' },
  { type: 'TEXT', label: 'Texto', icon: Type, description: 'Bloco de texto livre' },
  { type: 'IMAGE', label: 'Imagem', icon: ImageIcon, description: 'Imagem única' },
  { type: 'TWO_COLUMNS', label: 'Duas Colunas', icon: Columns, description: 'Layout em duas colunas' },
  { type: 'DIVIDER', label: 'Divisor', icon: Minus, description: 'Linha divisória' },
];

const getBlockIcon = (type: ProposalBlockType) => {
  const found = BLOCK_TYPES.find(b => b.type === type);
  return found ? found.icon : FileText;
};

const getBlockLabel = (type: ProposalBlockType) => {
  const found = BLOCK_TYPES.find(b => b.type === type);
  return found ? found.label : type;
};

function parseContent(content: string): BlockContent {
  try {
    return JSON.parse(content);
  } catch {
    return {};
  }
}

function stringifyContent(content: BlockContent): string {
  return JSON.stringify(content);
}

function parseStyle(style: string): TemplateStyle {
  try {
    return JSON.parse(style);
  } catch {
    return {};
  }
}

function replaceVariables(text: string, variables: Record<string, any>): string {
  if (!text) return '';
  return text.replace(/\{\{([^}]+)\}\}/g, (match, key) => {
    const keys = key.trim().split('.');
    let value: any = variables;
    for (const k of keys) {
      value = value?.[k];
      if (value === undefined) return match;
    }
    return String(value ?? match);
  });
}

function SortableBlock({ 
  block, 
  isSelected, 
  onClick, 
  onDelete,
  variables 
}: { 
  block: Block; 
  isSelected: boolean;
  onClick: () => void;
  onDelete: () => void;
  variables: Record<string, any>;
}) {
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

  const Icon = getBlockIcon(block.blockType);
  const content = parseContent(block.content);
  const blockStyle = parseStyle(block.style);

  return (
    <div
      ref={setNodeRef}
      style={style}
      className={cn(
        "group relative border rounded-lg bg-white transition-all",
        isSelected ? "ring-2 ring-primary border-primary" : "border-border hover:border-primary/50",
        !block.isVisible && "opacity-50"
      )}
      onClick={onClick}
      data-testid={`block-${block.id}`}
    >
      <div className="absolute left-2 top-1/2 -translate-y-1/2 opacity-0 group-hover:opacity-100 transition-opacity cursor-move" {...attributes} {...listeners}>
        <GripVertical className="h-5 w-5 text-muted-foreground" />
      </div>

      <div className="p-4 pl-10">
        <div className="flex items-center justify-between mb-2">
          <div className="flex items-center gap-2">
            <Icon className="h-4 w-4 text-muted-foreground" />
            <span className="text-sm font-medium">{getBlockLabel(block.blockType)}</span>
          </div>
          <Button
            variant="ghost"
            size="icon"
            className="h-6 w-6 opacity-0 group-hover:opacity-100 transition-opacity text-destructive hover:text-destructive"
            onClick={(e) => {
              e.stopPropagation();
              onDelete();
            }}
            data-testid={`delete-block-${block.id}`}
          >
            <Trash2 className="h-4 w-4" />
          </Button>
        </div>

        <BlockPreview block={block} content={content} variables={variables} />
      </div>
    </div>
  );
}

function BlockPreview({ 
  block, 
  content, 
  variables 
}: { 
  block: Block; 
  content: BlockContent; 
  variables: Record<string, any>;
}) {
  const { blockType } = block;
  const style = parseStyle(block.style);

  switch (blockType) {
    case 'HERO':
      return (
        <div className="bg-gradient-to-r from-primary/10 to-primary/5 p-4 rounded-lg text-center">
          <h3 className="text-lg font-bold">{replaceVariables(content.title || '{{proposal.title}}', variables)}</h3>
          <p className="text-sm text-muted-foreground mt-1">{replaceVariables(content.subtitle || 'Proposta para {{client.name}}', variables)}</p>
          {content.showDate && <p className="text-xs mt-2">{new Date().toLocaleDateString('pt-BR')}</p>}
        </div>
      );

    case 'ABOUT':
      return (
        <div className="space-y-2">
          <h4 className="font-semibold">{content.heading || 'Sobre Nós'}</h4>
          <p className="text-sm text-muted-foreground line-clamp-2">{content.description || 'Descrição da empresa...'}</p>
        </div>
      );

    case 'NEED':
      return (
        <div className="space-y-2">
          <h4 className="font-semibold">{content.heading || 'A Necessidade'}</h4>
          <p className="text-sm text-muted-foreground line-clamp-2">{replaceVariables(content.description || 'Descrição do problema...', variables)}</p>
        </div>
      );

    case 'SERVICES_TABLE':
      return (
        <div className="border rounded p-2">
          <div className="text-xs text-muted-foreground mb-1">Tabela de Serviços</div>
          <div className="flex justify-between text-sm">
            <span>{variables.proposal?.itemCount || 0} itens</span>
            <span className="font-semibold text-primary">{variables.proposal?.subtotal || 'R$ 0,00'}</span>
          </div>
        </div>
      );

    case 'INVESTMENT':
      return (
        <div className="bg-primary/5 p-3 rounded-lg text-center">
          <div className="text-sm text-muted-foreground">Investimento Total</div>
          <div className="text-xl font-bold text-primary">{variables.proposal?.total || 'R$ 0,00'}</div>
        </div>
      );

    case 'PORTFOLIO':
      return (
        <div className="grid grid-cols-3 gap-1">
          {[1, 2, 3].map(i => (
            <div key={i} className="aspect-square bg-muted rounded flex items-center justify-center">
              <ImageIcon className="h-4 w-4 text-muted-foreground" />
            </div>
          ))}
        </div>
      );

    case 'TESTIMONIAL':
      return (
        <div className="italic text-sm border-l-2 border-primary pl-3">
          "{content.quote || 'Depoimento do cliente...'}"
          <div className="mt-1 text-xs text-muted-foreground">- {content.author || 'Nome do Cliente'}</div>
        </div>
      );

    case 'TERMS':
      return (
        <div className="text-xs text-muted-foreground space-y-1">
          <p className="font-medium text-foreground">{content.heading || 'Termos e Condições'}</p>
          <p className="line-clamp-2">{content.text || 'Os termos e condições da proposta...'}</p>
        </div>
      );

    case 'SIGNATURE':
      return (
        <div className="flex justify-around text-center text-xs">
          <div className="space-y-1">
            <div className="w-24 border-t border-dashed pt-1">Empresa</div>
          </div>
          <div className="space-y-1">
            <div className="w-24 border-t border-dashed pt-1">Cliente</div>
          </div>
        </div>
      );

    case 'TEXT':
      return (
        <p className="text-sm text-muted-foreground line-clamp-3">
          {replaceVariables(content.text || 'Texto livre...', variables)}
        </p>
      );

    case 'IMAGE':
      return (
        <div className="aspect-video bg-muted rounded flex items-center justify-center">
          {content.url ? (
            <img src={content.url} alt={content.alt || ''} className="w-full h-full object-cover rounded" />
          ) : (
            <ImageIcon className="h-8 w-8 text-muted-foreground" />
          )}
        </div>
      );

    case 'TWO_COLUMNS':
      return (
        <div className="grid grid-cols-2 gap-2 text-xs text-muted-foreground">
          <div className="bg-muted/50 p-2 rounded">Coluna 1</div>
          <div className="bg-muted/50 p-2 rounded">Coluna 2</div>
        </div>
      );

    case 'DIVIDER':
      return <hr className="border-muted-foreground/20" />;

    default:
      return <div className="text-sm text-muted-foreground">Bloco: {blockType}</div>;
  }
}

function BlockEditor({ 
  block, 
  onUpdate 
}: { 
  block: Block; 
  onUpdate: (content: BlockContent, style?: TemplateStyle) => void;
}) {
  const content = parseContent(block.content);
  const style = parseStyle(block.style);

  const updateContent = (key: string, value: any) => {
    onUpdate({ ...content, [key]: value }, style);
  };

  const updateStyle = (key: string, value: any) => {
    onUpdate(content, { ...style, [key]: value });
  };

  switch (block.blockType) {
    case 'HERO':
      return (
        <div className="space-y-4">
          <div>
            <Label>Título</Label>
            <Input
              value={content.title || ''}
              onChange={(e) => updateContent('title', e.target.value)}
              placeholder="{{proposal.title}}"
              data-testid="editor-hero-title"
            />
            <p className="text-xs text-muted-foreground mt-1">Use {'{{proposal.title}}'} para o título da proposta</p>
          </div>
          <div>
            <Label>Subtítulo</Label>
            <Input
              value={content.subtitle || ''}
              onChange={(e) => updateContent('subtitle', e.target.value)}
              placeholder="Proposta para {{client.name}}"
              data-testid="editor-hero-subtitle"
            />
          </div>
          <div className="flex items-center gap-2">
            <Switch
              checked={content.showDate ?? true}
              onCheckedChange={(v) => updateContent('showDate', v)}
              data-testid="editor-hero-showdate"
            />
            <Label>Mostrar data</Label>
          </div>
          <div>
            <Label>Imagem de Fundo (URL)</Label>
            <Input
              value={content.backgroundImage || ''}
              onChange={(e) => updateContent('backgroundImage', e.target.value)}
              placeholder="https://..."
              data-testid="editor-hero-bg"
            />
          </div>
        </div>
      );

    case 'ABOUT':
      return (
        <div className="space-y-4">
          <div>
            <Label>Título</Label>
            <Input
              value={content.heading || ''}
              onChange={(e) => updateContent('heading', e.target.value)}
              placeholder="Sobre Nós"
              data-testid="editor-about-heading"
            />
          </div>
          <div>
            <Label>Descrição</Label>
            <Textarea
              value={content.description || ''}
              onChange={(e) => updateContent('description', e.target.value)}
              placeholder="Conte sobre sua empresa..."
              rows={4}
              data-testid="editor-about-description"
            />
          </div>
          <div>
            <Label>Destaques (separados por linha)</Label>
            <Textarea
              value={(content.highlights || []).join('\n')}
              onChange={(e) => updateContent('highlights', e.target.value.split('\n').filter(Boolean))}
              placeholder="10+ anos de experiência&#10;100+ projetos entregues"
              rows={3}
              data-testid="editor-about-highlights"
            />
          </div>
        </div>
      );

    case 'NEED':
      return (
        <div className="space-y-4">
          <div>
            <Label>Título</Label>
            <Input
              value={content.heading || ''}
              onChange={(e) => updateContent('heading', e.target.value)}
              placeholder="A Necessidade"
              data-testid="editor-need-heading"
            />
          </div>
          <div>
            <Label>Descrição do Problema</Label>
            <Textarea
              value={content.description || ''}
              onChange={(e) => updateContent('description', e.target.value)}
              placeholder="Descreva o problema ou necessidade do cliente..."
              rows={4}
              data-testid="editor-need-description"
            />
          </div>
        </div>
      );

    case 'SERVICES_TABLE':
      return (
        <div className="space-y-4">
          <div className="p-3 bg-muted/50 rounded-lg text-sm">
            <p className="font-medium">Tabela de Serviços</p>
            <p className="text-muted-foreground">Os itens são carregados automaticamente da proposta.</p>
          </div>
          <div className="flex items-center gap-2">
            <Switch
              checked={content.showQuantity ?? true}
              onCheckedChange={(v) => updateContent('showQuantity', v)}
            />
            <Label>Mostrar quantidade</Label>
          </div>
          <div className="flex items-center gap-2">
            <Switch
              checked={content.showDiscount ?? true}
              onCheckedChange={(v) => updateContent('showDiscount', v)}
            />
            <Label>Mostrar descontos</Label>
          </div>
        </div>
      );

    case 'INVESTMENT':
      return (
        <div className="space-y-4">
          <div>
            <Label>Título</Label>
            <Input
              value={content.heading || ''}
              onChange={(e) => updateContent('heading', e.target.value)}
              placeholder="Investimento Total"
              data-testid="editor-investment-heading"
            />
          </div>
          <div className="flex items-center gap-2">
            <Switch
              checked={content.showDiscount ?? true}
              onCheckedChange={(v) => updateContent('showDiscount', v)}
            />
            <Label>Mostrar desconto aplicado</Label>
          </div>
          <div className="flex items-center gap-2">
            <Switch
              checked={content.showValidUntil ?? true}
              onCheckedChange={(v) => updateContent('showValidUntil', v)}
            />
            <Label>Mostrar validade</Label>
          </div>
        </div>
      );

    case 'TESTIMONIAL':
      return (
        <div className="space-y-4">
          <div>
            <Label>Citação</Label>
            <Textarea
              value={content.quote || ''}
              onChange={(e) => updateContent('quote', e.target.value)}
              placeholder="O que o cliente disse..."
              rows={3}
              data-testid="editor-testimonial-quote"
            />
          </div>
          <div>
            <Label>Autor</Label>
            <Input
              value={content.author || ''}
              onChange={(e) => updateContent('author', e.target.value)}
              placeholder="Nome do Cliente"
              data-testid="editor-testimonial-author"
            />
          </div>
          <div>
            <Label>Empresa/Cargo</Label>
            <Input
              value={content.role || ''}
              onChange={(e) => updateContent('role', e.target.value)}
              placeholder="CEO, Empresa X"
              data-testid="editor-testimonial-role"
            />
          </div>
        </div>
      );

    case 'TERMS':
      return (
        <div className="space-y-4">
          <div>
            <Label>Título</Label>
            <Input
              value={content.heading || ''}
              onChange={(e) => updateContent('heading', e.target.value)}
              placeholder="Termos e Condições"
              data-testid="editor-terms-heading"
            />
          </div>
          <div>
            <Label>Texto dos Termos</Label>
            <Textarea
              value={content.text || ''}
              onChange={(e) => updateContent('text', e.target.value)}
              placeholder="Escreva os termos e condições..."
              rows={6}
              data-testid="editor-terms-text"
            />
          </div>
        </div>
      );

    case 'SIGNATURE':
      return (
        <div className="space-y-4">
          <div>
            <Label>Nome da Empresa</Label>
            <Input
              value={content.companyName || ''}
              onChange={(e) => updateContent('companyName', e.target.value)}
              placeholder="{{workspace.name}}"
              data-testid="editor-signature-company"
            />
          </div>
          <div className="flex items-center gap-2">
            <Switch
              checked={content.showDate ?? true}
              onCheckedChange={(v) => updateContent('showDate', v)}
            />
            <Label>Mostrar data de assinatura</Label>
          </div>
          <div className="flex items-center gap-2">
            <Switch
              checked={content.requireDigitalSignature ?? true}
              onCheckedChange={(v) => updateContent('requireDigitalSignature', v)}
            />
            <Label>Assinatura digital obrigatória</Label>
          </div>
        </div>
      );

    case 'TEXT':
      return (
        <div className="space-y-4">
          <div>
            <Label>Texto</Label>
            <Textarea
              value={content.text || ''}
              onChange={(e) => updateContent('text', e.target.value)}
              placeholder="Escreva seu texto aqui... Use {{client.name}} para variáveis"
              rows={6}
              data-testid="editor-text-content"
            />
          </div>
          <div>
            <Label>Alinhamento</Label>
            <Select
              value={style.textAlign || 'left'}
              onValueChange={(v) => updateStyle('textAlign', v)}
            >
              <SelectTrigger>
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="left">Esquerda</SelectItem>
                <SelectItem value="center">Centro</SelectItem>
                <SelectItem value="right">Direita</SelectItem>
                <SelectItem value="justify">Justificado</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>
      );

    case 'IMAGE':
      return (
        <div className="space-y-4">
          <div>
            <Label>URL da Imagem</Label>
            <Input
              value={content.url || ''}
              onChange={(e) => updateContent('url', e.target.value)}
              placeholder="https://..."
              data-testid="editor-image-url"
            />
          </div>
          <div>
            <Label>Texto Alternativo</Label>
            <Input
              value={content.alt || ''}
              onChange={(e) => updateContent('alt', e.target.value)}
              placeholder="Descrição da imagem"
              data-testid="editor-image-alt"
            />
          </div>
          <div>
            <Label>Legenda</Label>
            <Input
              value={content.caption || ''}
              onChange={(e) => updateContent('caption', e.target.value)}
              placeholder="Legenda opcional"
            />
          </div>
        </div>
      );

    case 'TWO_COLUMNS':
      return (
        <div className="space-y-4">
          <div>
            <Label>Coluna Esquerda</Label>
            <Textarea
              value={content.leftContent || ''}
              onChange={(e) => updateContent('leftContent', e.target.value)}
              placeholder="Conteúdo da coluna esquerda..."
              rows={3}
              data-testid="editor-columns-left"
            />
          </div>
          <div>
            <Label>Coluna Direita</Label>
            <Textarea
              value={content.rightContent || ''}
              onChange={(e) => updateContent('rightContent', e.target.value)}
              placeholder="Conteúdo da coluna direita..."
              rows={3}
              data-testid="editor-columns-right"
            />
          </div>
        </div>
      );

    case 'DIVIDER':
      return (
        <div className="space-y-4">
          <div>
            <Label>Estilo</Label>
            <Select
              value={style.dividerStyle || 'solid'}
              onValueChange={(v) => updateStyle('dividerStyle', v)}
            >
              <SelectTrigger>
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="solid">Sólido</SelectItem>
                <SelectItem value="dashed">Tracejado</SelectItem>
                <SelectItem value="dotted">Pontilhado</SelectItem>
              </SelectContent>
            </Select>
          </div>
          <div>
            <Label>Margem Vertical</Label>
            <Select
              value={style.margin || 'md'}
              onValueChange={(v) => updateStyle('margin', v)}
            >
              <SelectTrigger>
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="sm">Pequena</SelectItem>
                <SelectItem value="md">Média</SelectItem>
                <SelectItem value="lg">Grande</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>
      );

    case 'PORTFOLIO':
      return (
        <div className="space-y-4">
          <div>
            <Label>Título</Label>
            <Input
              value={content.heading || ''}
              onChange={(e) => updateContent('heading', e.target.value)}
              placeholder="Nosso Portfólio"
              data-testid="editor-portfolio-heading"
            />
          </div>
          <div>
            <Label>Imagens (URLs, uma por linha)</Label>
            <Textarea
              value={(content.images || []).join('\n')}
              onChange={(e) => updateContent('images', e.target.value.split('\n').filter(Boolean))}
              placeholder="https://exemplo.com/imagem1.jpg&#10;https://exemplo.com/imagem2.jpg"
              rows={4}
              data-testid="editor-portfolio-images"
            />
          </div>
          <div>
            <Label>Colunas</Label>
            <Select
              value={String(content.columns || 3)}
              onValueChange={(v) => updateContent('columns', parseInt(v))}
            >
              <SelectTrigger>
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="2">2 colunas</SelectItem>
                <SelectItem value="3">3 colunas</SelectItem>
                <SelectItem value="4">4 colunas</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>
      );

    default:
      return (
        <div className="text-sm text-muted-foreground">
          Editor não disponível para este tipo de bloco.
        </div>
      );
  }
}

export default function ProposalVisualBuilderPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [location, navigate] = useLocation();
  const params = useParams<{ slug: string; id: string }>();
  const entityId = params.id;
  
  const isTemplateMode = location.includes('/proposal-templates/');
  const proposalId = isTemplateMode ? undefined : entityId;
  const templateId = isTemplateMode ? entityId : undefined;

  const [blocks, setBlocks] = useState<Block[]>([]);
  const [selectedBlockId, setSelectedBlockId] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<'blocks' | 'style' | 'settings'>('blocks');
  const [hasChanges, setHasChanges] = useState(false);

  const sensors = useSensors(
    useSensor(PointerSensor),
    useSensor(KeyboardSensor, {
      coordinateGetter: sortableKeyboardCoordinates,
    })
  );

  const { data: visualData, isLoading: isLoadingProposal } = useQuery({
    queryKey: ['proposalVisual', currentWorkspace?.id, proposalId],
    queryFn: async () => {
      const res = await fetch(`/api/workspaces/${currentWorkspace!.id}/proposals/${proposalId}/visual`, {
        credentials: 'include',
      });
      if (!res.ok) throw new Error('Erro ao carregar proposta');
      return res.json();
    },
    enabled: !!currentWorkspace && !!proposalId && !isTemplateMode,
  });

  const { data: templateData, isLoading: isLoadingTemplate } = useQuery({
    queryKey: ['proposalTemplate', currentWorkspace?.id, templateId],
    queryFn: async () => {
      const res = await fetch(`/api/workspaces/${currentWorkspace!.id}/proposal-templates/${templateId}`, {
        credentials: 'include',
      });
      if (!res.ok) throw new Error('Erro ao carregar template');
      return res.json();
    },
    enabled: !!currentWorkspace && !!templateId && isTemplateMode,
  });

  const { data: proposalTemplates = [] } = useQuery({
    queryKey: ['proposalTemplates', currentWorkspace?.id],
    queryFn: async () => {
      const res = await fetch(`/api/workspaces/${currentWorkspace!.id}/proposal-templates`, {
        credentials: 'include',
      });
      if (!res.ok) throw new Error('Erro ao carregar templates');
      return res.json();
    },
    enabled: !!currentWorkspace && !isTemplateMode,
  });

  const isLoading = isTemplateMode ? isLoadingTemplate : isLoadingProposal;

  useEffect(() => {
    if (isTemplateMode && templateData?.blocks) {
      const mappedBlocks = templateData.blocks.map((b: any) => ({
        ...b,
        proposalId: templateId,
        content: b.defaultContent || b.content || '{}',
        style: b.defaultStyle || b.style || '{}',
      }));
      setBlocks(mappedBlocks);
    } else if (!isTemplateMode && visualData?.blocks) {
      setBlocks(visualData.blocks);
    }
  }, [visualData, templateData, isTemplateMode, templateId]);

  const variables = {
    proposal: {
      title: visualData?.proposal?.title || 'Título da Proposta',
      total: visualData?.items?.reduce((acc: number, item: any) => {
        const price = parseFloat(item.price) * item.quantity;
        const discount = parseFloat(item.discount || 0);
        const discountValue = item.discountType === 'PERCENT' ? price * discount / 100 : discount;
        return acc + (price - discountValue);
      }, 0)?.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }) || 'R$ 0,00',
      subtotal: visualData?.items?.reduce((acc: number, item: any) => acc + (parseFloat(item.price) * item.quantity), 0)?.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }) || 'R$ 0,00',
      itemCount: visualData?.items?.length || 0,
      validUntil: visualData?.proposal?.validUntil ? new Date(visualData.proposal.validUntil).toLocaleDateString('pt-BR') : 'N/A',
    },
    client: {
      name: visualData?.client?.companyName || 'Nome do Cliente',
      email: visualData?.client?.email || '',
      phone: visualData?.client?.phone || '',
    },
    workspace: {
      name: currentWorkspace?.name || 'Sua Empresa',
    },
  };

  const saveMutation = useMutation({
    mutationFn: async () => {
      if (isTemplateMode) {
        for (const block of blocks) {
          await fetch(`/api/workspaces/${currentWorkspace!.id}/proposal-templates/${templateId}/blocks/${block.id}`, {
            method: 'PATCH',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify({
              content: block.content,
              style: block.style,
              position: block.position,
            }),
          });
        }
        await fetch(`/api/workspaces/${currentWorkspace!.id}/proposal-templates/${templateId}/blocks/reorder`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          credentials: 'include',
          body: JSON.stringify({ orderedIds: blocks.map(b => b.id) }),
        });
      } else {
        for (const block of blocks) {
          await fetch(`/api/workspaces/${currentWorkspace!.id}/proposals/${proposalId}/blocks/${block.id}`, {
            method: 'PATCH',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify({
              content: block.content,
              style: block.style,
              position: block.position,
              isVisible: block.isVisible,
            }),
          });
        }
        await fetch(`/api/workspaces/${currentWorkspace!.id}/proposals/${proposalId}/blocks/reorder`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          credentials: 'include',
          body: JSON.stringify({ orderedIds: blocks.map(b => b.id) }),
        });
      }
    },
    onSuccess: () => {
      toast({ title: 'Sucesso', description: isTemplateMode ? 'Modelo salvo com sucesso!' : 'Proposta salva com sucesso!' });
      setHasChanges(false);
      if (isTemplateMode) {
        queryClient.invalidateQueries({ queryKey: ['proposalTemplate', currentWorkspace?.id, templateId] });
      } else {
        queryClient.invalidateQueries({ queryKey: ['proposalVisual', currentWorkspace?.id, proposalId] });
      }
    },
    onError: (error: any) => {
      toast({ title: 'Erro', description: error.message, variant: 'destructive' });
    },
  });

  const addBlockMutation = useMutation({
    mutationFn: async (blockType: ProposalBlockType) => {
      const endpoint = isTemplateMode 
        ? `/api/workspaces/${currentWorkspace!.id}/proposal-templates/${templateId}/blocks`
        : `/api/workspaces/${currentWorkspace!.id}/proposals/${proposalId}/blocks`;
      const payload = isTemplateMode 
        ? { blockType, position: blocks.length, content: '{}', style: '{}' }
        : { blockType, position: blocks.length, content: '{}', style: '{}', isVisible: true };
      const res = await fetch(endpoint, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify(payload),
      });
      if (!res.ok) throw new Error('Erro ao adicionar bloco');
      return res.json();
    },
    onSuccess: (newBlock) => {
      setBlocks(prev => [...prev, newBlock]);
      setSelectedBlockId(newBlock.id);
      setHasChanges(true);
    },
    onError: (error: any) => {
      toast({ title: 'Erro', description: error.message, variant: 'destructive' });
    },
  });

  const deleteBlockMutation = useMutation({
    mutationFn: async (blockId: string) => {
      const endpoint = isTemplateMode
        ? `/api/workspaces/${currentWorkspace!.id}/proposal-templates/${templateId}/blocks/${blockId}`
        : `/api/workspaces/${currentWorkspace!.id}/proposals/${proposalId}/blocks/${blockId}`;
      const res = await fetch(endpoint, {
        method: 'DELETE',
        credentials: 'include',
      });
      if (!res.ok) throw new Error('Erro ao excluir bloco');
    },
    onSuccess: (_, blockId) => {
      setBlocks(prev => prev.filter(b => b.id !== blockId));
      if (selectedBlockId === blockId) setSelectedBlockId(null);
      setHasChanges(true);
    },
    onError: (error: any) => {
      toast({ title: 'Erro', description: error.message, variant: 'destructive' });
    },
  });

  const applyTemplateMutation = useMutation({
    mutationFn: async (templateId: string) => {
      const res = await fetch(`/api/workspaces/${currentWorkspace!.id}/proposals/${proposalId}/apply-template`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ templateId }),
      });
      if (!res.ok) throw new Error('Erro ao aplicar template');
      return res.json();
    },
    onSuccess: (newBlocks) => {
      setBlocks(newBlocks);
      setSelectedBlockId(null);
      setHasChanges(false);
      toast({ title: 'Sucesso', description: 'Template aplicado com sucesso!' });
      queryClient.invalidateQueries({ queryKey: ['proposalVisual', currentWorkspace?.id, proposalId] });
    },
    onError: (error: any) => {
      toast({ title: 'Erro', description: error.message, variant: 'destructive' });
    },
  });

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;
    if (!over || active.id === over.id) return;

    setBlocks((items) => {
      const oldIndex = items.findIndex((i) => i.id === active.id);
      const newIndex = items.findIndex((i) => i.id === over.id);
      const reordered = arrayMove(items, oldIndex, newIndex);
      return reordered.map((b, i) => ({ ...b, position: i }));
    });
    setHasChanges(true);
  };

  const updateBlockContent = useCallback((blockId: string, content: BlockContent, style?: TemplateStyle) => {
    setBlocks(prev => prev.map(b => {
      if (b.id !== blockId) return b;
      return {
        ...b,
        content: stringifyContent(content),
        style: style ? JSON.stringify(style) : b.style,
      };
    }));
    setHasChanges(true);
  }, []);

  const selectedBlock = blocks.find(b => b.id === selectedBlockId);

  if (isLoading) {
    return (
      <div className="h-screen flex items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    );
  }

  return (
    <div className="h-screen flex flex-col bg-background">
      <header className="h-14 border-b flex items-center justify-between px-4 bg-card">
        <div className="flex items-center gap-4">
          <Button
            variant="ghost"
            size="icon"
            onClick={() => navigate(isTemplateMode ? `/${params.slug}/proposal-templates` : `/${params.slug}/proposals`)}
            data-testid="back-button"
          >
            <ArrowLeft className="h-5 w-5" />
          </Button>
          <div>
            <h1 className="font-semibold">
              {isTemplateMode 
                ? (templateData?.template?.name || 'Modelo') 
                : (visualData?.proposal?.title || 'Proposta')}
            </h1>
            <p className="text-xs text-muted-foreground">
              {isTemplateMode ? 'Editor de Modelo' : 'Editor Visual'}
            </p>
          </div>
        </div>

        <div className="flex items-center gap-2">
          {hasChanges && (
            <span className="text-xs text-amber-600">Alterações não salvas</span>
          )}
          {!isTemplateMode && (
            <Button
              variant="outline"
              size="sm"
              onClick={() => window.open(`/p/${visualData?.proposal?.publicToken}`, '_blank')}
              disabled={!visualData?.proposal?.publicToken}
              data-testid="preview-button"
            >
              <Eye className="h-4 w-4 mr-2" />
              Visualizar
            </Button>
          )}
          <Button
            size="sm"
            onClick={() => saveMutation.mutate()}
            disabled={saveMutation.isPending || !hasChanges}
            data-testid="save-button"
          >
            {saveMutation.isPending ? (
              <Loader2 className="h-4 w-4 mr-2 animate-spin" />
            ) : (
              <Save className="h-4 w-4 mr-2" />
            )}
            Salvar
          </Button>
        </div>
      </header>

      <div className="flex-1 flex overflow-hidden">
        <aside className="w-64 border-r bg-card flex flex-col">
          <Tabs value={activeTab} onValueChange={(v) => setActiveTab(v as any)} className="flex-1 flex flex-col">
            <TabsList className="m-2">
              <TabsTrigger value="blocks" className="flex-1">
                <LayoutGrid className="h-4 w-4 mr-1" />
                Blocos
              </TabsTrigger>
              <TabsTrigger value="style" className="flex-1">
                <Palette className="h-4 w-4 mr-1" />
                Estilo
              </TabsTrigger>
            </TabsList>

            <TabsContent value="blocks" className="flex-1 m-0 overflow-hidden">
              <ScrollArea className="h-full">
                <div className="p-2 space-y-1">
                  {BLOCK_TYPES.map((blockDef) => (
                    <Button
                      key={blockDef.type}
                      variant="ghost"
                      className="w-full justify-start h-auto py-2"
                      onClick={() => addBlockMutation.mutate(blockDef.type)}
                      disabled={addBlockMutation.isPending}
                      data-testid={`add-block-${blockDef.type.toLowerCase()}`}
                    >
                      <blockDef.icon className="h-4 w-4 mr-3 text-muted-foreground" />
                      <div className="text-left">
                        <div className="text-sm">{blockDef.label}</div>
                        <div className="text-xs text-muted-foreground">{blockDef.description}</div>
                      </div>
                    </Button>
                  ))}
                </div>
              </ScrollArea>
            </TabsContent>

            <TabsContent value="style" className="flex-1 m-0 p-4">
              <div className="space-y-4">
                <div>
                  <Label>Cor Principal</Label>
                  <div className="flex gap-2 mt-1">
                    <Input
                      type="color"
                      defaultValue="#22c55e"
                      className="w-12 h-9 p-1"
                    />
                    <Input defaultValue="#22c55e" className="flex-1" />
                  </div>
                </div>
                <div>
                  <Label>Cor Secundária</Label>
                  <div className="flex gap-2 mt-1">
                    <Input
                      type="color"
                      defaultValue="#eab308"
                      className="w-12 h-9 p-1"
                    />
                    <Input defaultValue="#eab308" className="flex-1" />
                  </div>
                </div>
                <div>
                  <Label>Fonte</Label>
                  <Select defaultValue="inter">
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="inter">Inter</SelectItem>
                      <SelectItem value="roboto">Roboto</SelectItem>
                      <SelectItem value="poppins">Poppins</SelectItem>
                      <SelectItem value="montserrat">Montserrat</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>
            </TabsContent>
          </Tabs>
        </aside>

        <main className="flex-1 bg-muted/30 overflow-auto p-6">
          <div className="max-w-3xl mx-auto space-y-3">
            {blocks.length === 0 ? (
              <div className="text-center py-12 border-2 border-dashed rounded-lg bg-card">
                <LayoutGrid className="h-12 w-12 mx-auto text-muted-foreground mb-4" />
                <h3 className="text-lg font-medium mb-2">Nenhum bloco adicionado</h3>
                <p className="text-muted-foreground mb-4">Clique em um bloco na barra lateral para adicionar</p>
                {proposalTemplates.length > 0 && (
                  <div className="mt-6 space-y-3">
                    <p className="text-sm text-muted-foreground">Ou aplique um template profissional:</p>
                    <Select onValueChange={(templateId) => applyTemplateMutation.mutate(templateId)}>
                      <SelectTrigger className="w-64 mx-auto" data-testid="select-apply-template">
                        <SelectValue placeholder="Selecione um template..." />
                      </SelectTrigger>
                      <SelectContent>
                        {proposalTemplates.map((template: any) => {
                          const templateStyle = template.style ? JSON.parse(template.style) : {};
                          return (
                            <SelectItem key={template.id} value={template.id}>
                              <span className="flex items-center gap-2">
                                <span 
                                  className="size-3 rounded-full inline-block"
                                  style={{ backgroundColor: templateStyle.primaryColor || '#666' }}
                                />
                                {template.name}
                              </span>
                            </SelectItem>
                          );
                        })}
                      </SelectContent>
                    </Select>
                  </div>
                )}
              </div>
            ) : (
              <DndContext
                sensors={sensors}
                collisionDetection={closestCenter}
                onDragEnd={handleDragEnd}
              >
                <SortableContext
                  items={blocks.map(b => b.id)}
                  strategy={verticalListSortingStrategy}
                >
                  {blocks.map((block) => (
                    <SortableBlock
                      key={block.id}
                      block={block}
                      isSelected={block.id === selectedBlockId}
                      onClick={() => setSelectedBlockId(block.id)}
                      onDelete={() => deleteBlockMutation.mutate(block.id)}
                      variables={variables}
                    />
                  ))}
                </SortableContext>
              </DndContext>
            )}
          </div>
        </main>

        <aside className="w-80 border-l bg-card flex flex-col">
          <div className="p-4 border-b">
            <h3 className="font-semibold">Propriedades</h3>
          </div>
          <ScrollArea className="flex-1">
            <div className="p-4">
              {selectedBlock ? (
                <>
                  <div className="flex items-center gap-2 mb-4">
                    {(() => {
                      const Icon = getBlockIcon(selectedBlock.blockType);
                      return <Icon className="h-5 w-5" />;
                    })()}
                    <span className="font-medium">{getBlockLabel(selectedBlock.blockType)}</span>
                  </div>

                  <div className="mb-4 flex items-center gap-2">
                    <Switch
                      checked={selectedBlock.isVisible}
                      onCheckedChange={(v) => {
                        setBlocks(prev => prev.map(b => 
                          b.id === selectedBlock.id ? { ...b, isVisible: v } : b
                        ));
                        setHasChanges(true);
                      }}
                    />
                    <Label>Visível</Label>
                  </div>

                  <BlockEditor
                    block={selectedBlock}
                    onUpdate={(content, style) => updateBlockContent(selectedBlock.id, content, style)}
                  />
                </>
              ) : (
                <div className="text-center text-muted-foreground py-8">
                  <Settings className="h-8 w-8 mx-auto mb-2 opacity-50" />
                  <p>Selecione um bloco para editar</p>
                </div>
              )}
            </div>
          </ScrollArea>

          <div className="p-4 border-t">
            <div className="text-xs text-muted-foreground">
              <p className="font-medium mb-1">Variáveis disponíveis:</p>
              <code className="block">{'{{client.name}}'}</code>
              <code className="block">{'{{proposal.title}}'}</code>
              <code className="block">{'{{proposal.total}}'}</code>
              <code className="block">{'{{workspace.name}}'}</code>
            </div>
          </div>
        </aside>
      </div>
    </div>
  );
}
