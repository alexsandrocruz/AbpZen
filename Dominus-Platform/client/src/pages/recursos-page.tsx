import { Link } from "wouter";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import {
  Users,
  FolderKanban,
  FileText,
  Receipt,
  Calendar,
  MessageSquare,
  Zap,
  Bot,
  Globe,
  CheckCircle2,
  ArrowRight,
  ArrowLeft,
  Building2,
  Target,
  FileSignature,
  DollarSign,
} from "lucide-react";

const allFeatures = [
  {
    id: "crm",
    icon: Users,
    title: "CRM Completo",
    subtitle: "Gerencie todos os seus clientes em um só lugar",
    color: "text-blue-500",
    bgColor: "bg-blue-500/10",
    description: "O módulo CRM do Dominus foi projetado para centralizar todas as informações dos seus clientes e leads. Com visualização em pipeline, você acompanha cada oportunidade do primeiro contato até o fechamento.",
    features: [
      "Cadastro completo de pessoas físicas e jurídicas",
      "Pipeline visual de vendas personalizável",
      "Histórico de interações e atividades",
      "Campos personalizados por cliente",
      "Integração com email e calendário",
      "Relatórios de conversão e performance",
    ],
  },
  {
    id: "projetos",
    icon: FolderKanban,
    title: "Gestão de Projetos",
    subtitle: "Organize entregas com quadros Kanban intuitivos",
    color: "text-green-500",
    bgColor: "bg-green-500/10",
    description: "Gerencie seus projetos de forma visual e eficiente. Crie quadros Kanban personalizados, defina prazos, acompanhe o progresso e mantenha sua equipe alinhada com as entregas.",
    features: [
      "Quadros Kanban drag-and-drop",
      "Controle de prazos e entregas",
      "Gestão de tarefas e subtarefas",
      "Registro de tempo por atividade",
      "Vinculação com clientes e contratos",
      "Relatórios de produtividade",
    ],
  },
  {
    id: "propostas",
    icon: FileText,
    title: "Propostas Profissionais",
    subtitle: "Impressione clientes com propostas elegantes",
    color: "text-purple-500",
    bgColor: "bg-purple-500/10",
    description: "Crie propostas comerciais impressionantes em minutos. Use modelos personalizáveis, adicione itens de serviço ou produtos, e envie links para aceite online com assinatura digital.",
    features: [
      "Templates personalizáveis",
      "Catálogo de produtos e serviços",
      "Link de visualização pública",
      "Notificação de visualização",
      "Aceite online com assinatura",
      "Conversão automática em contrato",
    ],
  },
  {
    id: "contratos",
    icon: FileSignature,
    title: "Contratos Digitais",
    subtitle: "Formalize acordos com segurança jurídica",
    color: "text-indigo-500",
    bgColor: "bg-indigo-500/10",
    description: "Transforme propostas aceitas em contratos formais. Configure parcelamentos, gere transações financeiras automáticas e colete assinaturas digitais dos seus clientes.",
    features: [
      "Geração a partir de propostas",
      "Parcelamento flexível (igual, percentual, manual)",
      "Geração automática de parcelas",
      "Assinatura digital integrada",
      "Vinculação com faturamento",
      "Histórico de alterações",
    ],
  },
  {
    id: "faturamento",
    icon: Receipt,
    title: "Faturamento Inteligente",
    subtitle: "Automatize a cobrança dos seus serviços",
    color: "text-orange-500",
    bgColor: "bg-orange-500/10",
    description: "Gere faturas profissionais a partir de contratos ou manualmente. Acompanhe pagamentos, envie lembretes automáticos e mantenha o controle da inadimplência.",
    features: [
      "Faturas automáticas de contratos",
      "Envio por email com link de pagamento",
      "Controle de status (pago, pendente, atrasado)",
      "Lembretes automáticos de vencimento",
      "Relatórios de faturamento",
      "Exportação para contabilidade",
    ],
  },
  {
    id: "financeiro",
    icon: DollarSign,
    title: "Financeiro Integrado",
    subtitle: "Controle total das suas finanças",
    color: "text-emerald-500",
    bgColor: "bg-emerald-500/10",
    description: "Tenha visão completa das contas a pagar e receber. Categorize transações, acompanhe o fluxo de caixa e tome decisões baseadas em dados reais do seu negócio.",
    features: [
      "Contas a pagar e receber",
      "Categorização automática",
      "Fluxo de caixa 12 meses",
      "Conciliação bancária",
      "Relatórios DRE e balancete",
      "Metas e orçamentos",
    ],
  },
  {
    id: "leads",
    icon: Target,
    title: "Captura de Leads",
    subtitle: "Converta visitantes em clientes",
    color: "text-red-500",
    bgColor: "bg-red-500/10",
    description: "Crie landing pages, formulários de captura e workflows automatizados. Cada lead entra direto no seu pipeline e segue um fluxo de nutrição até a conversão.",
    features: [
      "Construtor de landing pages",
      "Formulários personalizáveis",
      "Workflows de automação",
      "Scoring de leads",
      "Integração com email marketing",
      "Relatórios de conversão",
    ],
  },
  {
    id: "sites",
    icon: Globe,
    title: "Construtor de Sites",
    subtitle: "Crie presença online profissional",
    color: "text-cyan-500",
    bgColor: "bg-cyan-500/10",
    description: "Monte sites profissionais com nosso construtor visual. Adicione blog, portfólio, e publique com domínio personalizado. Tudo hospedado e otimizado para SEO.",
    features: [
      "Editor visual drag-and-drop",
      "Blog integrado com SEO",
      "Hospedagem inclusa",
      "Domínio personalizado",
      "Analytics integrado",
      "Templates responsivos",
    ],
  },
  {
    id: "automacao",
    icon: Zap,
    title: "Automações",
    subtitle: "Elimine tarefas repetitivas",
    color: "text-yellow-500",
    bgColor: "bg-yellow-500/10",
    description: "Crie workflows visuais para automatizar processos. Conecte gatilhos a ações e deixe o Dominus trabalhar enquanto você foca no que realmente importa.",
    features: [
      "Editor visual de workflows",
      "Gatilhos por evento ou tempo",
      "Ações em todas as entidades",
      "Condições e ramificações",
      "Integrações com Zapier/Make",
      "Logs de execução",
    ],
  },
  {
    id: "super-ai",
    icon: Bot,
    title: "Super AI",
    subtitle: "Assistente inteligente 24/7",
    color: "text-pink-500",
    bgColor: "bg-pink-500/10",
    description: "Use inteligência artificial para automatizar respostas, gerar conteúdo, analisar dados e tomar decisões mais rápidas. Seu assistente virtual está sempre disponível.",
    features: [
      "Chat com IA para análises",
      "Geração de conteúdo",
      "Respostas automáticas a clientes",
      "Análise de sentimento",
      "Sugestões inteligentes",
      "Integração com WhatsApp",
    ],
  },
  {
    id: "agendamentos",
    icon: Calendar,
    title: "Agendamentos",
    subtitle: "Organize reuniões sem esforço",
    color: "text-rose-500",
    bgColor: "bg-rose-500/10",
    description: "Compartilhe links de agendamento com seus clientes. Eles escolhem o melhor horário e você recebe notificações automáticas. Integração com Google Calendar.",
    features: [
      "Links de agendamento público",
      "Disponibilidade personalizável",
      "Confirmação automática",
      "Lembretes por email/SMS",
      "Sync com Google Calendar",
      "Múltiplos tipos de reunião",
    ],
  },
  {
    id: "colaboracao",
    icon: MessageSquare,
    title: "Colaboração",
    subtitle: "Comunique-se em tempo real",
    color: "text-teal-500",
    bgColor: "bg-teal-500/10",
    description: "Chat em tempo real com sua equipe e clientes. Compartilhe arquivos, comente em projetos e mantenha toda a comunicação centralizada no Dominus.",
    features: [
      "Chat em tempo real",
      "Canais por projeto/cliente",
      "Compartilhamento de arquivos",
      "Comentários em entidades",
      "Menções e notificações",
      "Histórico pesquisável",
    ],
  },
];

export default function RecursosPage() {
  return (
    <div className="min-h-screen bg-background">
      {/* Navigation */}
      <nav className="fixed top-0 left-0 right-0 z-50 bg-background/80 backdrop-blur-lg border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            <Link href="/">
              <div className="flex items-center gap-2 cursor-pointer">
                <div className="w-8 h-8 bg-primary rounded-lg flex items-center justify-center">
                  <Building2 className="h-5 w-5 text-primary-foreground" />
                </div>
                <span className="text-xl font-bold">Dominus</span>
              </div>
            </Link>
            <div className="flex items-center gap-4">
              <Link href="/login">
                <Button variant="ghost">Entrar</Button>
              </Link>
              <Link href="/login?register=true">
                <Button>Criar Conta</Button>
              </Link>
            </div>
          </div>
        </div>
      </nav>

      {/* Header */}
      <section className="pt-32 pb-12 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <Link href="/">
            <Button variant="ghost" className="mb-6">
              <ArrowLeft className="h-4 w-4 mr-2" />
              Voltar para Home
            </Button>
          </Link>
          <Badge variant="outline" className="mb-4">Recursos Completos</Badge>
          <h1 className="text-4xl sm:text-5xl font-bold mb-4">
            Conheça todas as funcionalidades
          </h1>
          <p className="text-xl text-muted-foreground max-w-3xl">
            O Dominus oferece uma suíte completa de ferramentas para gerenciar seu negócio. 
            Explore cada módulo em detalhes e descubra como podemos transformar sua gestão.
          </p>
        </div>
      </section>

      {/* Features List */}
      <section className="pb-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto space-y-16">
          {allFeatures.map((feature, index) => (
            <div 
              key={feature.id} 
              id={feature.id}
              className={`grid grid-cols-1 lg:grid-cols-2 gap-12 items-center ${
                index % 2 === 1 ? "lg:flex-row-reverse" : ""
              }`}
              data-testid={`feature-detail-${feature.id}`}
            >
              <div className={index % 2 === 1 ? "lg:order-2" : ""}>
                <div className={`w-16 h-16 rounded-xl ${feature.bgColor} flex items-center justify-center mb-6`}>
                  <feature.icon className={`h-8 w-8 ${feature.color}`} />
                </div>
                <h2 className="text-3xl font-bold mb-2">{feature.title}</h2>
                <p className="text-lg text-muted-foreground mb-4">{feature.subtitle}</p>
                <p className="text-muted-foreground mb-6">{feature.description}</p>
                <ul className="space-y-3 mb-6">
                  {feature.features.map((item, i) => (
                    <li key={i} className="flex items-center gap-3">
                      <CheckCircle2 className="h-5 w-5 text-green-500 shrink-0" />
                      <span>{item}</span>
                    </li>
                  ))}
                </ul>
                <Link href="/login?register=true">
                  <Button>
                    Experimentar {feature.title}
                    <ArrowRight className="ml-2 h-4 w-4" />
                  </Button>
                </Link>
              </div>
              <div className={`${index % 2 === 1 ? "lg:order-1" : ""}`}>
                <Card className="border-2 shadow-lg">
                  <CardContent className="p-8">
                    <div className="aspect-video bg-gradient-to-br from-muted to-muted/50 rounded-lg flex items-center justify-center">
                      <feature.icon className={`h-24 w-24 ${feature.color} opacity-50`} />
                    </div>
                  </CardContent>
                </Card>
              </div>
            </div>
          ))}
        </div>
      </section>

      {/* CTA */}
      <section className="py-20 px-4 sm:px-6 lg:px-8 bg-primary text-primary-foreground">
        <div className="max-w-4xl mx-auto text-center">
          <h2 className="text-3xl sm:text-4xl font-bold mb-4">
            Pronto para começar?
          </h2>
          <p className="text-xl opacity-90 mb-8">
            Teste todos os recursos por 14 dias grátis. Sem compromisso.
          </p>
          <Link href="/login?register=true">
            <Button size="lg" variant="secondary" className="text-lg px-8">
              Criar Conta Grátis
              <ArrowRight className="ml-2 h-5 w-5" />
            </Button>
          </Link>
        </div>
      </section>

      {/* Footer */}
      <footer className="py-8 px-4 sm:px-6 lg:px-8 border-t">
        <div className="max-w-7xl mx-auto flex flex-col md:flex-row items-center justify-between gap-4">
          <p className="text-sm text-muted-foreground">
            © {new Date().getFullYear()} Dominus. Um projeto da{" "}
            <a href="https://www.sapienzae.com.br" target="_blank" rel="noopener noreferrer" className="text-primary hover:underline">
              SAPIENZA Inteligência Digital
            </a>
          </p>
          <Link href="/">
            <Button variant="ghost" size="sm">
              <ArrowLeft className="h-4 w-4 mr-2" />
              Voltar para Home
            </Button>
          </Link>
        </div>
      </footer>
    </div>
  );
}
