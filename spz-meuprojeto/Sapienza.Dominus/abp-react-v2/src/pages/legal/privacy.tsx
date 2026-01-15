import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Shield, ArrowLeft } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Link } from "wouter";

export default function PrivacyPage() {
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
                        <h1 className="text-3xl font-bold tracking-tight">Política de Privacidade</h1>
                        <p className="text-muted-foreground">Última atualização: Janeiro de 2026</p>
                    </div>
                </div>

                <Card className="border-none shadow-xl">
                    <CardHeader>
                        <CardTitle className="flex items-center gap-2">
                            <Shield className="size-5 text-primary" />
                            Proteção de Dados e Privacidade (LGPD)
                        </CardTitle>
                    </CardHeader>
                    <CardContent className="prose prose-sm dark:prose-invert max-w-none">
                        <h2>1. Introdução</h2>
                        <p>
                            Esta Política de Privacidade descreve como coletamos, usamos, armazenamos e protegemos suas informações pessoais em conformidade com a Lei Geral de Proteção de Dados (LGPD - Lei nº 13.709/2018).
                        </p>

                        <h2>2. Dados Coletados</h2>
                        <p>
                            Podemos coletar os seguintes tipos de dados pessoais:
                        </p>
                        <ul>
                            <li><strong>Dados de identificação:</strong> nome, e-mail, telefone</li>
                            <li><strong>Dados de acesso:</strong> endereço IP, logs de acesso, dispositivo utilizado</li>
                            <li><strong>Dados de uso:</strong> preferências, histórico de navegação na plataforma</li>
                        </ul>

                        <h2>3. Finalidade do Tratamento</h2>
                        <p>
                            Seus dados são tratados para as seguintes finalidades:
                        </p>
                        <ul>
                            <li>Prestação e melhoria dos serviços</li>
                            <li>Comunicação sobre atualizações e novidades</li>
                            <li>Cumprimento de obrigações legais</li>
                            <li>Segurança e prevenção de fraudes</li>
                        </ul>

                        <h2>4. Base Legal</h2>
                        <p>
                            O tratamento de dados baseia-se em: consentimento do titular, execução de contrato, cumprimento de obrigação legal e legítimo interesse.
                        </p>

                        <h2>5. Compartilhamento de Dados</h2>
                        <p>
                            Seus dados podem ser compartilhados com:
                        </p>
                        <ul>
                            <li>Prestadores de serviços essenciais (hospedagem, analytics)</li>
                            <li>Autoridades competentes quando exigido por lei</li>
                        </ul>

                        <h2>6. Seus Direitos (LGPD)</h2>
                        <p>
                            Você tem direito a:
                        </p>
                        <ul>
                            <li>Confirmar a existência de tratamento</li>
                            <li>Acessar seus dados</li>
                            <li>Corrigir dados incompletos ou desatualizados</li>
                            <li>Solicitar anonimização, bloqueio ou eliminação</li>
                            <li>Solicitar portabilidade dos dados</li>
                            <li>Revogar consentimento</li>
                        </ul>

                        <h2>7. Segurança</h2>
                        <p>
                            Adotamos medidas técnicas e organizacionais para proteger seus dados contra acessos não autorizados, perda ou destruição.
                        </p>

                        <h2>8. Retenção de Dados</h2>
                        <p>
                            Seus dados são mantidos pelo tempo necessário para cumprir as finalidades descritas ou conforme exigido por lei.
                        </p>

                        <h2>9. Contato do Encarregado (DPO)</h2>
                        <p>
                            Para exercer seus direitos ou esclarecer dúvidas sobre o tratamento de dados, entre em contato com nosso Encarregado de Proteção de Dados: <strong>dpo@zensuite.com.br</strong>
                        </p>
                    </CardContent>
                </Card>
            </div>

            {/* Background decoration */}
            <div className="fixed inset-0 -z-10 bg-[radial-gradient(ellipse_at_top_right,_var(--tw-gradient-stops))] from-primary/10 via-background to-background" />
        </div>
    );
}
