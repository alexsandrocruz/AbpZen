import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { FileText, ArrowLeft } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Link } from "wouter";

export default function TermsPage() {
    return (
        <div className="min-h-screen bg-background p-4 md:p-8">
            <div className="max-w-4xl mx-auto space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 pb-12">
                <div className="flex items-center gap-4">
                    <Link href="/auth/register">
                        <Button variant="ghost" size="icon" className="shrink-0">
                            <ArrowLeft className="size-5" />
                        </Button>
                    </Link>
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Termos de Uso</h1>
                        <p className="text-muted-foreground">Última atualização: Janeiro de 2026</p>
                    </div>
                </div>

                <Card className="border-none shadow-xl">
                    <CardHeader>
                        <CardTitle className="flex items-center gap-2">
                            <FileText className="size-5 text-primary" />
                            Termos e Condições de Uso
                        </CardTitle>
                    </CardHeader>
                    <CardContent className="prose prose-sm dark:prose-invert max-w-none">
                        <h2>1. Aceitação dos Termos</h2>
                        <p>
                            Ao acessar e utilizar esta plataforma, você concorda em cumprir e estar vinculado aos seguintes termos e condições de uso. Se você não concordar com qualquer parte destes termos, não deverá usar nossos serviços.
                        </p>

                        <h2>2. Descrição do Serviço</h2>
                        <p>
                            Nossa plataforma oferece ferramentas de gestão empresarial, incluindo mas não limitado a: gerenciamento de usuários, controle de acesso, análise de dados e relatórios.
                        </p>

                        <h2>3. Cadastro e Conta</h2>
                        <p>
                            Para utilizar determinados recursos, você precisará criar uma conta. Você é responsável por manter a confidencialidade de suas credenciais e por todas as atividades realizadas em sua conta.
                        </p>

                        <h2>4. Uso Aceitável</h2>
                        <p>
                            Você concorda em usar a plataforma apenas para fins legais e de acordo com estes termos. É proibido:
                        </p>
                        <ul>
                            <li>Violar leis ou regulamentos aplicáveis</li>
                            <li>Interferir na segurança ou integridade do sistema</li>
                            <li>Transmitir conteúdo malicioso ou não autorizado</li>
                            <li>Tentar acessar recursos sem autorização</li>
                        </ul>

                        <h2>5. Propriedade Intelectual</h2>
                        <p>
                            Todo o conteúdo da plataforma, incluindo software, textos, gráficos e logotipos, é protegido por direitos autorais e outras leis de propriedade intelectual.
                        </p>

                        <h2>6. Limitação de Responsabilidade</h2>
                        <p>
                            A plataforma é fornecida "como está", sem garantias de qualquer tipo. Não nos responsabilizamos por danos diretos, indiretos, incidentais ou consequentes decorrentes do uso ou impossibilidade de uso do serviço.
                        </p>

                        <h2>7. Alterações nos Termos</h2>
                        <p>
                            Reservamo-nos o direito de modificar estes termos a qualquer momento. As alterações entrarão em vigor imediatamente após a publicação. O uso continuado após alterações constitui aceitação dos novos termos.
                        </p>

                        <h2>8. Contato</h2>
                        <p>
                            Para questões sobre estes termos, entre em contato através do e-mail: <strong>suporte@zensuite.com.br</strong>
                        </p>
                    </CardContent>
                </Card>
            </div>

            {/* Background decoration */}
            <div className="fixed inset-0 -z-10 bg-[radial-gradient(ellipse_at_top_right,_var(--tw-gradient-stops))] from-primary/10 via-background to-background" />
        </div>
    );
}
