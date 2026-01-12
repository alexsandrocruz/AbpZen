import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Separator } from "@/components/ui/separator";
import { AppShell } from "@/components/layout/shell";
import { CreditCard, Check, Zap, Users, HardDrive } from "lucide-react";
import { useParams } from "wouter";

export default function BillingPage() {
  const { slug } = useParams<{ slug: string }>();

  const currentPlan = {
    name: "Gratuito",
    price: "R$ 0",
    period: "/mês",
  };

  const plans = [
    {
      name: "Gratuito",
      price: "R$ 0",
      period: "/mês",
      features: ["1 usuário", "100 clientes", "10 projetos", "1GB armazenamento"],
      current: true,
    },
    {
      name: "Profissional",
      price: "R$ 49",
      period: "/mês",
      features: ["5 usuários", "Clientes ilimitados", "Projetos ilimitados", "10GB armazenamento", "Suporte prioritário"],
      popular: true,
    },
    {
      name: "Empresarial",
      price: "R$ 99",
      period: "/mês",
      features: ["Usuários ilimitados", "Clientes ilimitados", "Projetos ilimitados", "100GB armazenamento", "Suporte 24/7", "API personalizada"],
    },
  ];

  const usage = {
    users: { current: 1, max: 1 },
    clients: { current: 12, max: 100 },
    projects: { current: 3, max: 10 },
    storage: { current: 0.2, max: 1 },
  };

  return (
    <AppShell>
      <div className="container max-w-5xl py-8">
        <div className="mb-8">
          <h1 className="text-3xl font-bold" data-testid="text-page-title">Cobrança</h1>
          <p className="text-muted-foreground mt-1">Gerencie seu plano e faturamento</p>
        </div>

        <div className="space-y-6">
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <CreditCard className="h-5 w-5" />
                Plano Atual
              </CardTitle>
              <CardDescription>
                Você está no plano {currentPlan.name}
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="flex items-center justify-between">
                <div>
                  <div className="flex items-baseline gap-1">
                    <span className="text-4xl font-bold">{currentPlan.price}</span>
                    <span className="text-muted-foreground">{currentPlan.period}</span>
                  </div>
                  <Badge variant="secondary" className="mt-2">Plano Gratuito</Badge>
                </div>
                <Button data-testid="button-upgrade">
                  <Zap className="mr-2 h-4 w-4" />
                  Fazer Upgrade
                </Button>
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Uso do Plano</CardTitle>
              <CardDescription>
                Veja quanto você está usando dos recursos disponíveis
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-6">
              <div className="space-y-2">
                <div className="flex items-center justify-between text-sm">
                  <div className="flex items-center gap-2">
                    <Users className="h-4 w-4 text-muted-foreground" />
                    <span>Usuários</span>
                  </div>
                  <span>{usage.users.current} de {usage.users.max}</span>
                </div>
                <Progress value={(usage.users.current / usage.users.max) * 100} />
              </div>

              <div className="space-y-2">
                <div className="flex items-center justify-between text-sm">
                  <div className="flex items-center gap-2">
                    <Users className="h-4 w-4 text-muted-foreground" />
                    <span>Clientes</span>
                  </div>
                  <span>{usage.clients.current} de {usage.clients.max}</span>
                </div>
                <Progress value={(usage.clients.current / usage.clients.max) * 100} />
              </div>

              <div className="space-y-2">
                <div className="flex items-center justify-between text-sm">
                  <div className="flex items-center gap-2">
                    <HardDrive className="h-4 w-4 text-muted-foreground" />
                    <span>Armazenamento</span>
                  </div>
                  <span>{usage.storage.current}GB de {usage.storage.max}GB</span>
                </div>
                <Progress value={(usage.storage.current / usage.storage.max) * 100} />
              </div>
            </CardContent>
          </Card>

          <Separator />

          <div>
            <h2 className="text-xl font-semibold mb-4">Planos Disponíveis</h2>
            <div className="grid md:grid-cols-3 gap-4">
              {plans.map((plan) => (
                <Card 
                  key={plan.name} 
                  className={plan.popular ? "border-primary shadow-lg" : ""}
                  data-testid={`card-plan-${plan.name.toLowerCase()}`}
                >
                  <CardHeader>
                    {plan.popular && (
                      <Badge className="w-fit mb-2">Mais Popular</Badge>
                    )}
                    <CardTitle>{plan.name}</CardTitle>
                    <div className="flex items-baseline gap-1">
                      <span className="text-3xl font-bold">{plan.price}</span>
                      <span className="text-muted-foreground">{plan.period}</span>
                    </div>
                  </CardHeader>
                  <CardContent>
                    <ul className="space-y-2">
                      {plan.features.map((feature) => (
                        <li key={feature} className="flex items-center gap-2 text-sm">
                          <Check className="h-4 w-4 text-primary" />
                          {feature}
                        </li>
                      ))}
                    </ul>
                    <Button 
                      className="w-full mt-4" 
                      variant={plan.current ? "outline" : "default"}
                      disabled={plan.current}
                    >
                      {plan.current ? "Plano Atual" : "Selecionar"}
                    </Button>
                  </CardContent>
                </Card>
              ))}
            </div>
          </div>

          <Card className="bg-muted/50">
            <CardContent className="pt-6">
              <p className="text-sm text-muted-foreground text-center">
                Esta é uma versão de demonstração. A funcionalidade completa de pagamentos será implementada em breve.
              </p>
            </CardContent>
          </Card>
        </div>
      </div>
    </AppShell>
  );
}
