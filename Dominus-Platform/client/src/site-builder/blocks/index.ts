import { LucideIcon, Layout, Layers, Star, DollarSign, MousePointer, HelpCircle, Menu, FileText, Image, FormInput, BookOpen, Box } from 'lucide-react';

export interface BlockDefinition {
  key: string;
  label: string;
  description: string;
  category: string;
  icon: LucideIcon;
  defaultProps: Record<string, any>;
  schema: BlockPropSchema[];
}

export interface BlockPropSchema {
  key: string;
  label: string;
  type: 'text' | 'textarea' | 'number' | 'boolean' | 'color' | 'select' | 'image' | 'form-select';
  options?: { value: string; label: string }[];
  defaultValue?: any;
}

export const BLOCK_REGISTRY: BlockDefinition[] = [
  {
    key: 'hero-simple',
    label: 'Hero Simples',
    description: 'Seção hero com título, subtítulo e botão CTA',
    category: 'HERO',
    icon: Layout,
    defaultProps: {
      title: 'Bem-vindo ao nosso site',
      subtitle: 'Descubra como podemos ajudar você a alcançar seus objetivos',
      ctaText: 'Começar Agora',
      ctaLink: '#',
      backgroundColor: '#3B82F6',
      textColor: '#FFFFFF',
      alignment: 'center',
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'subtitle', label: 'Subtítulo', type: 'textarea' },
      { key: 'ctaText', label: 'Texto do Botão', type: 'text' },
      { key: 'ctaLink', label: 'Link do Botão', type: 'text' },
      { key: 'backgroundColor', label: 'Cor de Fundo', type: 'color' },
      { key: 'textColor', label: 'Cor do Texto', type: 'color' },
      { key: 'alignment', label: 'Alinhamento', type: 'select', options: [
        { value: 'left', label: 'Esquerda' },
        { value: 'center', label: 'Centro' },
        { value: 'right', label: 'Direita' },
      ]},
    ],
  },
  {
    key: 'hero-with-image',
    label: 'Hero com Imagem',
    description: 'Seção hero com imagem lateral',
    category: 'HERO',
    icon: Layout,
    defaultProps: {
      title: 'Transforme sua ideia em realidade',
      subtitle: 'Soluções personalizadas para o seu negócio',
      ctaText: 'Saiba Mais',
      ctaLink: '#',
      imageUrl: 'https://images.unsplash.com/photo-1551434678-e076c223a692?w=800',
      imagePosition: 'right',
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'subtitle', label: 'Subtítulo', type: 'textarea' },
      { key: 'ctaText', label: 'Texto do Botão', type: 'text' },
      { key: 'ctaLink', label: 'Link do Botão', type: 'text' },
      { key: 'imageUrl', label: 'URL da Imagem', type: 'image' },
      { key: 'imagePosition', label: 'Posição da Imagem', type: 'select', options: [
        { value: 'left', label: 'Esquerda' },
        { value: 'right', label: 'Direita' },
      ]},
    ],
  },
  {
    key: 'features-grid',
    label: 'Grade de Recursos',
    description: 'Grade com ícones e descrições de recursos',
    category: 'FEATURES',
    icon: Layers,
    defaultProps: {
      title: 'Nossos Recursos',
      subtitle: 'Tudo o que você precisa em um só lugar',
      features: [
        { icon: 'zap', title: 'Rápido', description: 'Performance otimizada' },
        { icon: 'shield', title: 'Seguro', description: 'Proteção de dados' },
        { icon: 'heart', title: 'Confiável', description: 'Suporte dedicado' },
        { icon: 'star', title: 'Premium', description: 'Qualidade garantida' },
      ],
      columns: 4,
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'subtitle', label: 'Subtítulo', type: 'textarea' },
      { key: 'columns', label: 'Colunas', type: 'select', options: [
        { value: '2', label: '2 Colunas' },
        { value: '3', label: '3 Colunas' },
        { value: '4', label: '4 Colunas' },
      ]},
    ],
  },
  {
    key: 'features-list',
    label: 'Lista de Recursos',
    description: 'Lista vertical com recursos e descrições',
    category: 'FEATURES',
    icon: Layers,
    defaultProps: {
      title: 'Por que nos escolher?',
      features: [
        { title: 'Experiência', description: 'Mais de 10 anos no mercado' },
        { title: 'Qualidade', description: 'Padrões internacionais' },
        { title: 'Suporte', description: 'Atendimento 24/7' },
      ],
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
    ],
  },
  {
    key: 'testimonials-carousel',
    label: 'Carrossel de Depoimentos',
    description: 'Depoimentos de clientes em formato carrossel',
    category: 'TESTIMONIALS',
    icon: Star,
    defaultProps: {
      title: 'O que nossos clientes dizem',
      testimonials: [
        { name: 'João Silva', role: 'CEO', company: 'Tech Corp', quote: 'Excelente serviço!', avatar: '' },
        { name: 'Maria Santos', role: 'Diretora', company: 'StartupXYZ', quote: 'Superou nossas expectativas.', avatar: '' },
      ],
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
    ],
  },
  {
    key: 'testimonials-grid',
    label: 'Grade de Depoimentos',
    description: 'Depoimentos em formato de cards',
    category: 'TESTIMONIALS',
    icon: Star,
    defaultProps: {
      title: 'Depoimentos',
      testimonials: [
        { name: 'Pedro Costa', role: 'Gerente', quote: 'Transformou nosso negócio!', rating: 5 },
        { name: 'Ana Lima', role: 'Fundadora', quote: 'Recomendo fortemente.', rating: 5 },
        { name: 'Carlos Oliveira', role: 'Diretor', quote: 'Parceria de sucesso.', rating: 5 },
      ],
      columns: 3,
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'columns', label: 'Colunas', type: 'select', options: [
        { value: '2', label: '2 Colunas' },
        { value: '3', label: '3 Colunas' },
      ]},
    ],
  },
  {
    key: 'pricing-table',
    label: 'Tabela de Preços',
    description: 'Comparativo de planos e preços',
    category: 'PRICING',
    icon: DollarSign,
    defaultProps: {
      title: 'Nossos Planos',
      subtitle: 'Escolha o plano ideal para você',
      plans: [
        { name: 'Básico', price: 'R$ 99', period: '/mês', features: ['5 usuários', '10GB', 'Suporte email'], highlighted: false },
        { name: 'Pro', price: 'R$ 199', period: '/mês', features: ['25 usuários', '100GB', 'Suporte prioritário'], highlighted: true },
        { name: 'Enterprise', price: 'R$ 499', period: '/mês', features: ['Ilimitado', '1TB', 'Suporte dedicado'], highlighted: false },
      ],
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'subtitle', label: 'Subtítulo', type: 'textarea' },
    ],
  },
  {
    key: 'cta-simple',
    label: 'CTA Simples',
    description: 'Call to action com texto e botão',
    category: 'CTA',
    icon: MousePointer,
    defaultProps: {
      title: 'Pronto para começar?',
      subtitle: 'Entre em contato conosco hoje mesmo',
      ctaText: 'Fale Conosco',
      ctaLink: '#',
      backgroundColor: '#10B981',
      textColor: '#FFFFFF',
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'subtitle', label: 'Subtítulo', type: 'textarea' },
      { key: 'ctaText', label: 'Texto do Botão', type: 'text' },
      { key: 'ctaLink', label: 'Link do Botão', type: 'text' },
      { key: 'backgroundColor', label: 'Cor de Fundo', type: 'color' },
      { key: 'textColor', label: 'Cor do Texto', type: 'color' },
    ],
  },
  {
    key: 'cta-with-form',
    label: 'CTA com Formulário',
    description: 'Call to action integrado com formulário de lead',
    category: 'CTA',
    icon: MousePointer,
    defaultProps: {
      title: 'Receba uma proposta',
      subtitle: 'Preencha o formulário e entraremos em contato',
      formId: '',
      backgroundColor: '#1F2937',
      textColor: '#FFFFFF',
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'subtitle', label: 'Subtítulo', type: 'textarea' },
      { key: 'formId', label: 'Formulário', type: 'form-select' },
      { key: 'backgroundColor', label: 'Cor de Fundo', type: 'color' },
      { key: 'textColor', label: 'Cor do Texto', type: 'color' },
    ],
  },
  {
    key: 'faq-accordion',
    label: 'FAQ Accordion',
    description: 'Perguntas frequentes em formato accordion',
    category: 'FAQ',
    icon: HelpCircle,
    defaultProps: {
      title: 'Perguntas Frequentes',
      items: [
        { question: 'Como funciona?', answer: 'É muito simples...' },
        { question: 'Qual o prazo de entrega?', answer: 'Normalmente entre 5 a 10 dias úteis.' },
        { question: 'Posso cancelar?', answer: 'Sim, a qualquer momento.' },
      ],
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
    ],
  },
  {
    key: 'header-simple',
    label: 'Cabeçalho Simples',
    description: 'Navegação simples com logo e links',
    category: 'HEADER',
    icon: Menu,
    defaultProps: {
      logoText: 'Minha Empresa',
      logoUrl: '',
      links: [
        { label: 'Início', href: '/' },
        { label: 'Sobre', href: '/sobre' },
        { label: 'Contato', href: '/contato' },
      ],
      ctaText: 'Entrar',
      ctaLink: '/login',
    },
    schema: [
      { key: 'logoText', label: 'Texto do Logo', type: 'text' },
      { key: 'logoUrl', label: 'URL do Logo', type: 'image' },
      { key: 'ctaText', label: 'Texto do Botão', type: 'text' },
      { key: 'ctaLink', label: 'Link do Botão', type: 'text' },
    ],
  },
  {
    key: 'footer-simple',
    label: 'Rodapé Simples',
    description: 'Rodapé com links e copyright',
    category: 'FOOTER',
    icon: Menu,
    defaultProps: {
      companyName: 'Minha Empresa',
      links: [
        { label: 'Política de Privacidade', href: '/privacidade' },
        { label: 'Termos de Uso', href: '/termos' },
      ],
      socialLinks: {
        instagram: '',
        facebook: '',
        linkedin: '',
      },
    },
    schema: [
      { key: 'companyName', label: 'Nome da Empresa', type: 'text' },
    ],
  },
  {
    key: 'content-text',
    label: 'Bloco de Texto',
    description: 'Conteúdo de texto formatado',
    category: 'CONTENT',
    icon: FileText,
    defaultProps: {
      title: '',
      content: '<p>Seu conteúdo aqui...</p>',
      alignment: 'left',
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'content', label: 'Conteúdo', type: 'textarea' },
      { key: 'alignment', label: 'Alinhamento', type: 'select', options: [
        { value: 'left', label: 'Esquerda' },
        { value: 'center', label: 'Centro' },
        { value: 'right', label: 'Direita' },
      ]},
    ],
  },
  {
    key: 'content-image',
    label: 'Imagem',
    description: 'Imagem com legenda opcional',
    category: 'CONTENT',
    icon: Image,
    defaultProps: {
      imageUrl: 'https://images.unsplash.com/photo-1486312338219-ce68d2c6f44d?w=800',
      alt: 'Descrição da imagem',
      caption: '',
      fullWidth: true,
    },
    schema: [
      { key: 'imageUrl', label: 'URL da Imagem', type: 'image' },
      { key: 'alt', label: 'Texto Alternativo', type: 'text' },
      { key: 'caption', label: 'Legenda', type: 'text' },
      { key: 'fullWidth', label: 'Largura Total', type: 'boolean' },
    ],
  },
  {
    key: 'gallery-grid',
    label: 'Galeria de Imagens',
    description: 'Grade de imagens com lightbox',
    category: 'GALLERY',
    icon: Image,
    defaultProps: {
      title: 'Galeria',
      images: [
        { url: 'https://images.unsplash.com/photo-1551434678-e076c223a692?w=400', alt: 'Imagem 1' },
        { url: 'https://images.unsplash.com/photo-1486312338219-ce68d2c6f44d?w=400', alt: 'Imagem 2' },
        { url: 'https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=400', alt: 'Imagem 3' },
      ],
      columns: 3,
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'columns', label: 'Colunas', type: 'select', options: [
        { value: '2', label: '2 Colunas' },
        { value: '3', label: '3 Colunas' },
        { value: '4', label: '4 Colunas' },
      ]},
    ],
  },
  {
    key: 'form-embed',
    label: 'Formulário de Lead',
    description: 'Incorpora um formulário de captura de leads',
    category: 'FORM',
    icon: FormInput,
    defaultProps: {
      title: 'Entre em Contato',
      subtitle: 'Preencha o formulário abaixo',
      formId: '',
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'subtitle', label: 'Subtítulo', type: 'textarea' },
      { key: 'formId', label: 'Formulário', type: 'form-select' },
    ],
  },
  {
    key: 'blog-recent',
    label: 'Posts Recentes',
    description: 'Lista de posts recentes do blog',
    category: 'BLOG',
    icon: BookOpen,
    defaultProps: {
      title: 'Últimas do Blog',
      limit: 3,
      showExcerpt: true,
      showDate: true,
    },
    schema: [
      { key: 'title', label: 'Título', type: 'text' },
      { key: 'limit', label: 'Quantidade', type: 'number' },
      { key: 'showExcerpt', label: 'Mostrar Resumo', type: 'boolean' },
      { key: 'showDate', label: 'Mostrar Data', type: 'boolean' },
    ],
  },
  {
    key: 'spacer',
    label: 'Espaçador',
    description: 'Espaço vertical entre blocos',
    category: 'UTILITY',
    icon: Box,
    defaultProps: {
      height: 64,
    },
    schema: [
      { key: 'height', label: 'Altura (px)', type: 'number' },
    ],
  },
  {
    key: 'divider',
    label: 'Divisor',
    description: 'Linha divisória horizontal',
    category: 'UTILITY',
    icon: Box,
    defaultProps: {
      style: 'solid',
      color: '#E5E7EB',
      width: '100%',
    },
    schema: [
      { key: 'style', label: 'Estilo', type: 'select', options: [
        { value: 'solid', label: 'Sólido' },
        { value: 'dashed', label: 'Tracejado' },
        { value: 'dotted', label: 'Pontilhado' },
      ]},
      { key: 'color', label: 'Cor', type: 'color' },
      { key: 'width', label: 'Largura', type: 'text' },
    ],
  },
];

export function getBlockByKey(key: string): BlockDefinition | undefined {
  return BLOCK_REGISTRY.find(b => b.key === key);
}

export function getBlocksByCategory(category: string): BlockDefinition[] {
  return BLOCK_REGISTRY.filter(b => b.category === category);
}

export function getAllCategories(): string[] {
  return Array.from(new Set(BLOCK_REGISTRY.map(b => b.category)));
}
