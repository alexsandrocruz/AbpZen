import { useState } from "react";
import { Link } from "wouter";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Badge } from "@/components/ui/badge";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import {
  Users,
  FolderKanban,
  FileText,
  Receipt,
  TrendingUp,
  Calendar,
  MessageSquare,
  Zap,
  Bot,
  Globe,
  CheckCircle2,
  Star,
  ArrowRight,
  Menu,
  X,
  Shield,
  Clock,
  BarChart3,
  Target,
  Mail,
  Phone,
  Briefcase,
  FileSignature,
  DollarSign,
  Building2,
  Sparkles,
  Smartphone,
} from "lucide-react";
import { useMutation } from "@tanstack/react-query";
import { useToast } from "@/hooks/use-toast";
import dashboardMockup from "@assets/generated_images/business_dashboard_mockup_interface.png";
import automationIllustration from "@assets/generated_images/business_automation_isometric_illustration.png";

const featureBlocks = [
  {
    title: "Venda com profissionalismo",
    message: "Feche mais negócios com menos esforço.",
    color: "text-blue-500",
    bgColor: "bg-blue-500/10",
    features: [
      { icon: Users, title: "CRM simples e visual", description: "Gerencie clientes, leads e oportunidades com histórico completo de interações." },
      { icon: FileText, title: "Propostas com aceite online", description: "Crie propostas impressionantes e acompanhe visualizações em tempo real." },
      { icon: FileSignature, title: "Contratos digitais automáticos", description: "Transforme propostas em contratos com parcelas e assinatura digital." },
    ],
  },
  {
    title: "Execute sem perder controle",
    message: "Saiba exatamente o que está sendo entregue.",
    color: "text-green-500",
    bgColor: "bg-green-500/10",
    features: [
      { icon: FolderKanban, title: "Projetos com Kanban", description: "Organize projetos com quadros visuais, cronogramas e acompanhamento de entregas." },
      { icon: Target, title: "Tarefas vinculadas", description: "Conecte tarefas a clientes e contratos para visibilidade total do progresso." },
      { icon: Calendar, title: "Agendamentos integrados", description: "Calendário com lembretes automáticos e sincronização com Google Calendar." },
    ],
  },
  {
    title: "Receba com previsibilidade",
    message: "Veja o dinheiro antes do problema aparecer.",
    color: "text-emerald-500",
    bgColor: "bg-emerald-500/10",
    features: [
      { icon: Receipt, title: "Faturamento automático", description: "Gere faturas a partir de contratos com controle de recebimentos." },
      { icon: DollarSign, title: "Contas a pagar e receber", description: "Gerencie seu financeiro com categorização automática e relatórios." },
      { icon: TrendingUp, title: "Fluxo de caixa em tempo real", description: "Visualize sua saúde financeira e antecipe problemas de caixa." },
    ],
  },
  {
    title: "Automatize e ganhe tempo",
    message: "Menos trabalho manual, mais foco no crescimento.",
    color: "text-purple-500",
    bgColor: "bg-purple-500/10",
    features: [
      { icon: Zap, title: "Workflows visuais", description: "Automatize processos com Email, SMS e WhatsApp integrados.", badge: "SMS/WhatsApp BETA" },
      { icon: Bot, title: "Super AI — Copiloto Operacional", description: "Gere propostas, responda clientes e automatize rotinas com IA." },
      { icon: Globe, title: "Integrações e site próprio", description: "Conecte ferramentas externas e crie seu site com hospedagem inclusa." },
    ],
  },
];

const testimonials = [
  {
    name: "Marina Santos",
    role: "CEO, Agência Criativa",
    highlight: "Economizamos 10 horas por semana",
    content: "O Dominus transformou nossa gestão. Antes usávamos 5 ferramentas diferentes, agora tudo está integrado.",
    avatar: "MS",
  },
  {
    name: "Ricardo Oliveira",
    role: "Freelancer de Design",
    highlight: "Fechamos 40% mais projetos",
    content: "As propostas profissionais e o aceite online fizeram toda diferença. Os clientes adoram a organização.",
    avatar: "RO",
  },
  {
    name: "Fernanda Lima",
    role: "Diretora, Consultoria Tech",
    highlight: "Agora sabemos a rentabilidade de cada cliente",
    content: "O controle financeiro integrado com projetos mudou nosso jogo. Visibilidade total do negócio.",
    avatar: "FL",
  },
];

const faqs = [
  {
    question: "Posso testar gratuitamente?",
    answer: "Sim! Oferecemos 14 dias de teste gratuito com acesso a todas as funcionalidades. Não é necessário cartão de crédito para começar.",
  },
  {
    question: "Como funciona a migração de dados?",
    answer: "Nossa equipe oferece suporte completo para migrar seus dados de outras ferramentas. Importamos clientes, projetos e histórico de forma segura.",
  },
  {
    question: "O Dominus é seguro?",
    answer: "Absolutamente. Utilizamos criptografia de ponta a ponta, backups automáticos e infraestrutura em nuvem com certificações de segurança internacionais.",
  },
  {
    question: "Posso cancelar a qualquer momento?",
    answer: "Sim, não há fidelidade. Você pode cancelar sua assinatura quando quiser e seus dados ficam disponíveis para exportação por 30 dias.",
  },
  {
    question: "Tem app para celular?",
    answer: "O Dominus é 100% responsivo e funciona perfeitamente em qualquer dispositivo. Em breve lançaremos apps nativos para iOS e Android.",
  },
  {
    question: "Como funciona o suporte?",
    answer: "Oferecemos suporte via chat, email e videoconferência. Clientes do plano Enterprise têm gerente de sucesso dedicado.",
  },
];

const plans = [
  {
    name: "Starter",
    price: "R$ 117",
    description: "Para quem está organizando o negócio",
    features: [
      "Até 50 clientes",
      "3 usuários",
      "Propostas e contratos",
      "Faturamento básico",
      "Suporte por email",
    ],
  },
  {
    name: "Professional",
    price: "R$ 197",
    description: "Mais escolhido por estúdios e agências",
    popular: true,
    features: [
      "Clientes ilimitados",
      "10 usuários",
      "Automação de processos",
      "IA para produtividade",
      "Captação de leads e site",
      "SMS e WhatsApp integrados",
      "Suporte prioritário",
    ],
  },
  {
    name: "Enterprise",
    price: "R$ 497",
    description: "Para quem quer escalar operação e marca",
    features: [
      "Tudo do Professional",
      "Usuários ilimitados",
      "Super AI ilimitado",
      "Sua própria plataforma (White-label)",
      "API completa para integrações",
      "Gerente de sucesso dedicado",
      "SLA garantido",
    ],
  },
];

export default function LandingPage() {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const [leadForm, setLeadForm] = useState({ name: "", email: "", company: "", message: "" });
  const { toast } = useToast();

  const submitLead = useMutation({
    mutationFn: async (data: typeof leadForm) => {
      const response = await fetch("/api/leads/landing", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });
      if (!response.ok) throw new Error("Erro ao enviar");
      return response.json();
    },
    onSuccess: () => {
      toast({ title: "Mensagem enviada!", description: "Entraremos em contato em breve." });
      setLeadForm({ name: "", email: "", company: "", message: "" });
    },
    onError: () => {
      toast({ title: "Erro", description: "Não foi possível enviar. Tente novamente.", variant: "destructive" });
    },
  });

  const handleSubmitLead = (e: React.FormEvent) => {
    e.preventDefault();
    submitLead.mutate(leadForm);
  };

  return (
    <div className="min-h-screen bg-background">
      {/* Navigation */}
      <nav className="fixed top-0 left-0 right-0 z-50 bg-background/80 backdrop-blur-lg border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            <div className="flex items-center gap-2">
              <div className="w-8 h-8 bg-primary rounded-lg flex items-center justify-center">
                <Building2 className="h-5 w-5 text-primary-foreground" />
              </div>
              <span className="text-xl font-bold">Dominus</span>
            </div>

            {/* Desktop Menu */}
            <div className="hidden md:flex items-center gap-8">
              <a href="#recursos" className="text-muted-foreground hover:text-foreground transition-colors">
                Recursos
              </a>
              <a href="#precos" className="text-muted-foreground hover:text-foreground transition-colors">
                Preços
              </a>
              <a href="#depoimentos" className="text-muted-foreground hover:text-foreground transition-colors">
                Depoimentos
              </a>
              <a href="#faq" className="text-muted-foreground hover:text-foreground transition-colors">
                FAQ
              </a>
              <a href="#contato" className="text-muted-foreground hover:text-foreground transition-colors">
                Contato
              </a>
            </div>

            <div className="hidden md:flex items-center gap-4">
              <Link href="/login">
                <Button variant="ghost" data-testid="nav-login">Entrar</Button>
              </Link>
              <Link href="/login?register=true">
                <Button data-testid="nav-register">Criar Conta</Button>
              </Link>
            </div>

            {/* Mobile Menu Button */}
            <button
              className="md:hidden p-2"
              onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
              data-testid="mobile-menu-toggle"
            >
              {mobileMenuOpen ? <X className="h-6 w-6" /> : <Menu className="h-6 w-6" />}
            </button>
          </div>
        </div>

        {/* Mobile Menu */}
        {mobileMenuOpen && (
          <div className="md:hidden border-t bg-background">
            <div className="px-4 py-4 space-y-4">
              <a href="#recursos" className="block text-muted-foreground hover:text-foreground">
                Recursos
              </a>
              <a href="#precos" className="block text-muted-foreground hover:text-foreground">
                Preços
              </a>
              <a href="#depoimentos" className="block text-muted-foreground hover:text-foreground">
                Depoimentos
              </a>
              <a href="#faq" className="block text-muted-foreground hover:text-foreground">
                FAQ
              </a>
              <a href="#contato" className="block text-muted-foreground hover:text-foreground">
                Contato
              </a>
              <div className="flex gap-4 pt-4 border-t">
                <Link href="/login">
                  <Button variant="ghost" className="flex-1">Entrar</Button>
                </Link>
                <Link href="/login?register=true">
                  <Button className="flex-1">Criar Conta</Button>
                </Link>
              </div>
            </div>
          </div>
        )}
      </nav>

      {/* Hero Section */}
      <section className="pt-32 pb-20 px-4 sm:px-6 lg:px-8 relative overflow-hidden">
        <div className="absolute inset-0 bg-gradient-to-br from-primary/5 via-transparent to-primary/10" />
        <div className="max-w-7xl mx-auto relative">
          <div className="text-center max-w-4xl mx-auto">
            <Badge variant="secondary" className="mb-6">
              <Sparkles className="h-3 w-3 mr-1" />
              Novo: Super AI — seu copiloto operacional
            </Badge>
            <h1 className="text-4xl sm:text-5xl lg:text-6xl font-bold tracking-tight mb-6">
              Do primeiro contato ao{" "}
              <span className="text-primary">dinheiro no caixa</span>
              {" "}— tudo conectado
            </h1>
            <p className="text-xl text-muted-foreground mb-6 max-w-2xl mx-auto">
              CRM, propostas, contratos, projetos, faturamento e automações em uma única plataforma feita para negócios de serviços.
            </p>
            <ul className="flex flex-col sm:flex-row items-center justify-center gap-4 sm:gap-6 text-muted-foreground mb-8">
              <li className="flex items-center gap-2">
                <CheckCircle2 className="h-5 w-5 text-green-500" />
                <span>Substitua planilhas e várias ferramentas</span>
              </li>
              <li className="flex items-center gap-2">
                <CheckCircle2 className="h-5 w-5 text-green-500" />
                <span>Automatize rotinas com IA</span>
              </li>
              <li className="flex items-center gap-2">
                <CheckCircle2 className="h-5 w-5 text-green-500" />
                <span>Previsibilidade financeira</span>
              </li>
            </ul>
            <div className="flex flex-col sm:flex-row items-center justify-center gap-4">
              <Link href="/login?register=true">
                <Button size="lg" className="text-lg px-8 h-12" data-testid="hero-cta-register">
                  Começar Grátis por 14 Dias
                  <ArrowRight className="ml-2 h-5 w-5" />
                </Button>
              </Link>
              <Link href="/login">
                <Button size="lg" variant="outline" className="text-lg px-8 h-12" data-testid="hero-cta-login">
                  Já tenho conta
                </Button>
              </Link>
            </div>
            <p className="text-sm text-muted-foreground mt-4">
              Sem cartão de crédito • Cancele quando quiser
            </p>
          </div>

          {/* Dashboard Preview */}
          <div className="mt-16 relative">
            <div className="bg-gradient-to-t from-background via-transparent to-transparent absolute inset-0 z-10 pointer-events-none" />
            <div className="rounded-xl border shadow-2xl overflow-hidden bg-card">
              <div className="p-4 bg-muted/50 border-b flex items-center gap-2">
                <div className="w-3 h-3 rounded-full bg-red-500" />
                <div className="w-3 h-3 rounded-full bg-yellow-500" />
                <div className="w-3 h-3 rounded-full bg-green-500" />
                <span className="text-sm text-muted-foreground ml-4">app.dominus.com.br</span>
              </div>
              <img 
                src={dashboardMockup} 
                alt="Dashboard Dominus - Gestão empresarial completa" 
                className="w-full h-auto"
              />
            </div>
          </div>
        </div>
      </section>

      {/* Social Proof */}
      <section className="py-12 border-y bg-muted/30">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex flex-wrap items-center justify-center gap-8 text-muted-foreground">
            <div className="flex items-center gap-2">
              <Shield className="h-5 w-5" />
              <span>Dados criptografados</span>
            </div>
            <div className="flex items-center gap-2">
              <Clock className="h-5 w-5" />
              <span>Suporte em até 2h</span>
            </div>
            <div className="flex items-center gap-2">
              <Users className="h-5 w-5" />
              <span>+500 empresas</span>
            </div>
            <div className="flex items-center gap-2">
              <Star className="h-5 w-5 text-yellow-500" />
              <span>4.9/5 avaliação</span>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section id="recursos" className="py-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <div className="text-center mb-16">
            <Badge variant="outline" className="mb-4">Recursos</Badge>
            <h2 className="text-3xl sm:text-4xl font-bold mb-4">
              Tudo que você precisa em um só lugar
            </h2>
            <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
              Chega de pagar por várias ferramentas. O Dominus integra todas as funcionalidades 
              essenciais para gerenciar seu negócio.
            </p>
          </div>

          <div className="space-y-16">
            {featureBlocks.map((block, blockIndex) => (
              <div key={blockIndex} data-testid={`feature-block-${blockIndex}`}>
                <div className="flex items-center gap-3 mb-6">
                  <div className={`w-1 h-8 rounded-full ${block.bgColor.replace('/10', '')}`} />
                  <div>
                    <h3 className={`text-xl font-bold ${block.color}`}>{block.title}</h3>
                    <p className="text-muted-foreground">{block.message}</p>
                  </div>
                </div>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                  {block.features.map((feature, featureIndex) => (
                    <Card key={featureIndex} className="group hover:shadow-lg transition-shadow" data-testid={`feature-card-${blockIndex}-${featureIndex}`}>
                      <CardHeader>
                        <div className={`w-12 h-12 rounded-lg ${block.bgColor} flex items-center justify-center mb-4`}>
                          <feature.icon className={`h-6 w-6 ${block.color}`} />
                        </div>
                        <CardTitle className="flex items-center gap-2 text-lg">
                          {feature.title}
                          {'badge' in feature && feature.badge && (
                            <Badge variant="secondary" className="text-xs px-1.5 py-0.5 bg-amber-500/10 text-amber-600 border-amber-500/20">
                              {feature.badge}
                            </Badge>
                          )}
                        </CardTitle>
                      </CardHeader>
                      <CardContent>
                        <p className="text-muted-foreground text-sm">{feature.description}</p>
                      </CardContent>
                    </Card>
                  ))}
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Automation Highlight Section */}
      <section className="py-20 px-4 sm:px-6 lg:px-8 bg-muted/30">
        <div className="max-w-7xl mx-auto">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-12 items-center">
            <div>
              <Badge variant="outline" className="mb-4">Automação</Badge>
              <h2 className="text-3xl sm:text-4xl font-bold mb-4">
                Automatize processos e ganhe tempo
              </h2>
              <p className="text-xl text-muted-foreground mb-6">
                Conecte todas as áreas do seu negócio em fluxos automatizados. 
                Da captação de leads à emissão de faturas, tudo funciona em harmonia.
              </p>
              <ul className="space-y-4">
                {[
                  { text: "Workflows visuais sem código", beta: false },
                  { text: "Envio de emails automáticos", beta: false },
                  { text: "Notificações por SMS", beta: true },
                  { text: "Mensagens via WhatsApp", beta: true },
                  { text: "Integração com ferramentas externas", beta: false },
                  { text: "Relatórios automáticos", beta: false },
                ].map((item, i) => (
                  <li key={i} className="flex items-center gap-3">
                    <CheckCircle2 className="h-5 w-5 text-green-500 shrink-0" />
                    <span className="flex items-center gap-2">
                      {item.text}
                      {item.beta && (
                        <Badge variant="secondary" className="text-xs px-1.5 py-0.5 bg-amber-500/10 text-amber-600 border-amber-500/20">
                          BETA
                        </Badge>
                      )}
                    </span>
                  </li>
                ))}
              </ul>
              <Link href="/login?register=true">
                <Button size="lg" className="mt-8">
                  Começar Agora
                  <ArrowRight className="ml-2 h-5 w-5" />
                </Button>
              </Link>
            </div>
            <div className="relative">
              <div className="absolute inset-0 bg-gradient-to-r from-primary/20 to-transparent rounded-2xl -rotate-3" />
              <img 
                src={automationIllustration} 
                alt="Automação de processos empresariais" 
                className="relative rounded-xl shadow-xl w-full h-auto"
              />
            </div>
          </div>
        </div>
      </section>

      {/* Pricing Section */}
      <section id="precos" className="py-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <div className="text-center mb-16">
            <Badge variant="outline" className="mb-4">Preços</Badge>
            <h2 className="text-3xl sm:text-4xl font-bold mb-4">
              Planos para cada momento do seu negócio
            </h2>
            <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
              Comece grátis e escale conforme cresce. Sem taxas ocultas.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 max-w-5xl mx-auto">
            {plans.map((plan, i) => (
              <Card
                key={i}
                className={`relative ${plan.popular ? "border-primary shadow-lg scale-105" : ""}`}
                data-testid={`plan-card-${plan.name.toLowerCase()}`}
              >
                {plan.popular && (
                  <div className="absolute -top-3 left-1/2 -translate-x-1/2">
                    <Badge className="bg-primary">Mais Popular</Badge>
                  </div>
                )}
                <CardHeader className="text-center pb-2">
                  <CardTitle className="text-2xl">{plan.name}</CardTitle>
                  <p className="text-muted-foreground">{plan.description}</p>
                </CardHeader>
                <CardContent className="text-center">
                  <div className="mb-6">
                    <span className="text-4xl font-bold">{plan.price}</span>
                    <span className="text-muted-foreground">/mês</span>
                  </div>
                  <ul className="space-y-3 text-left mb-6">
                    {plan.features.map((feature, j) => (
                      <li key={j} className="flex items-center gap-2">
                        <CheckCircle2 className="h-5 w-5 text-green-500 shrink-0" />
                        <span>{feature}</span>
                      </li>
                    ))}
                  </ul>
                  <Link href="/login?register=true">
                    <Button
                      className="w-full"
                      variant={plan.popular ? "default" : "outline"}
                    >
                      Começar Agora
                    </Button>
                  </Link>
                </CardContent>
              </Card>
            ))}
          </div>
        </div>
      </section>

      {/* Testimonials Section */}
      <section id="depoimentos" className="py-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <div className="text-center mb-16">
            <Badge variant="outline" className="mb-4">Depoimentos</Badge>
            <h2 className="text-3xl sm:text-4xl font-bold mb-4">
              O que nossos clientes dizem
            </h2>
            <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
              Histórias reais de quem transformou a gestão do negócio com o Dominus.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {testimonials.map((testimonial, i) => (
              <Card key={i} className="relative" data-testid={`testimonial-${i}`}>
                <CardContent className="pt-6">
                  <div className="flex items-center gap-1 mb-2">
                    {[...Array(5)].map((_, j) => (
                      <Star key={j} className="h-5 w-5 fill-yellow-400 text-yellow-400" />
                    ))}
                  </div>
                  <p className="font-semibold text-lg text-primary mb-3">"{testimonial.highlight}"</p>
                  <p className="text-muted-foreground mb-6">{testimonial.content}</p>
                  <div className="flex items-center gap-3">
                    <div className="w-10 h-10 rounded-full bg-primary/10 flex items-center justify-center text-primary font-semibold">
                      {testimonial.avatar}
                    </div>
                    <div>
                      <p className="font-semibold">{testimonial.name}</p>
                      <p className="text-sm text-muted-foreground">{testimonial.role}</p>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </div>
      </section>

      {/* FAQ Section */}
      <section id="faq" className="py-20 px-4 sm:px-6 lg:px-8 bg-muted/30">
        <div className="max-w-3xl mx-auto">
          <div className="text-center mb-16">
            <Badge variant="outline" className="mb-4">FAQ</Badge>
            <h2 className="text-3xl sm:text-4xl font-bold mb-4">
              Perguntas Frequentes
            </h2>
            <p className="text-xl text-muted-foreground">
              Tire suas dúvidas sobre o Dominus.
            </p>
          </div>

          <Accordion type="single" collapsible className="space-y-4">
            {faqs.map((faq, i) => (
              <AccordionItem key={i} value={`faq-${i}`} className="bg-card rounded-lg border px-4" data-testid={`faq-${i}`}>
                <AccordionTrigger className="text-left hover:no-underline">
                  {faq.question}
                </AccordionTrigger>
                <AccordionContent className="text-muted-foreground">
                  {faq.answer}
                </AccordionContent>
              </AccordionItem>
            ))}
          </Accordion>
        </div>
      </section>

      {/* Contact / Lead Form Section */}
      <section id="contato" className="py-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
            <div>
              <Badge variant="outline" className="mb-4">Contato</Badge>
              <h2 className="text-3xl sm:text-4xl font-bold mb-4">
                Fale com nossa equipe
              </h2>
              <p className="text-xl text-muted-foreground mb-8">
                Quer saber mais sobre como o Dominus pode ajudar seu negócio? 
                Preencha o formulário e entraremos em contato.
              </p>

              <div className="space-y-4">
                <div className="flex items-center gap-4">
                  <div className="w-12 h-12 rounded-lg bg-primary/10 flex items-center justify-center">
                    <Mail className="h-6 w-6 text-primary" />
                  </div>
                  <div>
                    <p className="font-semibold">Email</p>
                    <p className="text-muted-foreground">contato@dominus.app</p>
                  </div>
                </div>
                <div className="flex items-center gap-4">
                  <div className="w-12 h-12 rounded-lg bg-primary/10 flex items-center justify-center">
                    <Phone className="h-6 w-6 text-primary" />
                  </div>
                  <div>
                    <p className="font-semibold">WhatsApp</p>
                    <p className="text-muted-foreground">(11) 99999-9999</p>
                  </div>
                </div>
                <div className="flex items-center gap-4">
                  <div className="w-12 h-12 rounded-lg bg-primary/10 flex items-center justify-center">
                    <Briefcase className="h-6 w-6 text-primary" />
                  </div>
                  <div>
                    <p className="font-semibold">Horário</p>
                    <p className="text-muted-foreground">Seg-Sex, 9h às 18h</p>
                  </div>
                </div>
              </div>
            </div>

            <Card>
              <CardHeader>
                <CardTitle>Solicite uma demonstração</CardTitle>
              </CardHeader>
              <CardContent>
                <form onSubmit={handleSubmitLead} className="space-y-4">
                  <div>
                    <Input
                      placeholder="Seu nome"
                      value={leadForm.name}
                      onChange={(e) => setLeadForm({ ...leadForm, name: e.target.value })}
                      required
                      data-testid="lead-name"
                    />
                  </div>
                  <div>
                    <Input
                      type="email"
                      placeholder="Seu email"
                      value={leadForm.email}
                      onChange={(e) => setLeadForm({ ...leadForm, email: e.target.value })}
                      required
                      data-testid="lead-email"
                    />
                  </div>
                  <div>
                    <Input
                      placeholder="Empresa (opcional)"
                      value={leadForm.company}
                      onChange={(e) => setLeadForm({ ...leadForm, company: e.target.value })}
                      data-testid="lead-company"
                    />
                  </div>
                  <div>
                    <Textarea
                      placeholder="Como podemos ajudar?"
                      value={leadForm.message}
                      onChange={(e) => setLeadForm({ ...leadForm, message: e.target.value })}
                      rows={4}
                      data-testid="lead-message"
                    />
                  </div>
                  <Button
                    type="submit"
                    className="w-full"
                    disabled={submitLead.isPending}
                    data-testid="lead-submit"
                  >
                    {submitLead.isPending ? "Enviando..." : "Enviar Mensagem"}
                  </Button>
                </form>
              </CardContent>
            </Card>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="py-20 px-4 sm:px-6 lg:px-8 bg-primary text-primary-foreground">
        <div className="max-w-4xl mx-auto text-center">
          <h2 className="text-3xl sm:text-4xl font-bold mb-4">
            Pronto para transformar sua gestão?
          </h2>
          <p className="text-xl opacity-90 mb-8">
            Junte-se a centenas de empresas que já simplificaram seus processos com o Dominus.
          </p>
          <div className="flex flex-col sm:flex-row items-center justify-center gap-4">
            <Link href="/login?register=true">
              <Button size="lg" variant="secondary" className="text-lg px-8" data-testid="cta-register">
                Criar Conta Grátis
                <ArrowRight className="ml-2 h-5 w-5" />
              </Button>
            </Link>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="py-12 px-4 sm:px-6 lg:px-8 border-t">
        <div className="max-w-7xl mx-auto">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8 mb-12">
            <div className="md:col-span-2">
              <div className="flex items-center gap-2 mb-4">
                <div className="w-8 h-8 bg-primary rounded-lg flex items-center justify-center">
                  <Building2 className="h-5 w-5 text-primary-foreground" />
                </div>
                <span className="text-xl font-bold">Dominus</span>
              </div>
              <p className="text-muted-foreground mb-4">
                A plataforma completa de gestão para freelancers e agências. 
                Simplifique seus processos e foque no que realmente importa: seu negócio.
              </p>
              <p className="text-sm text-muted-foreground">
                Um projeto da{" "}
                <a
                  href="https://www.sapienzae.com.br"
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-primary hover:underline font-medium"
                >
                  SAPIENZA Inteligência Digital
                </a>
              </p>
            </div>
            <div>
              <h4 className="font-semibold mb-4">Produto</h4>
              <ul className="space-y-2 text-muted-foreground">
                <li><a href="#recursos" className="hover:text-foreground">Recursos</a></li>
                <li><a href="#precos" className="hover:text-foreground">Preços</a></li>
                <li><a href="#depoimentos" className="hover:text-foreground">Depoimentos</a></li>
                <li><a href="#faq" className="hover:text-foreground">FAQ</a></li>
              </ul>
            </div>
            <div>
              <h4 className="font-semibold mb-4">Legal</h4>
              <ul className="space-y-2 text-muted-foreground">
                <li><a href="#" className="hover:text-foreground">Termos de Uso</a></li>
                <li><a href="#" className="hover:text-foreground">Política de Privacidade</a></li>
                <li><a href="#" className="hover:text-foreground">LGPD</a></li>
              </ul>
            </div>
          </div>

          <div className="border-t pt-8 flex flex-col md:flex-row items-center justify-between gap-4">
            <p className="text-sm text-muted-foreground">
              © {new Date().getFullYear()} Dominus. Todos os direitos reservados.
            </p>
            <p className="text-sm text-muted-foreground">
              Feito com ❤️ no Brasil por{" "}
              <a
                href="https://www.sapienzae.com.br"
                target="_blank"
                rel="noopener noreferrer"
                className="text-primary hover:underline"
              >
                SAPIENZA Inteligência Digital
              </a>
            </p>
          </div>
        </div>
      </footer>
    </div>
  );
}
