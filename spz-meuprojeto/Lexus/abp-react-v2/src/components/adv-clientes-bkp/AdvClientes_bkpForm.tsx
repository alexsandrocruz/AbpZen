import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateAdvClientes_bkp, useUpdateAdvClientes_bkp } from "@/lib/abp/hooks/useAdvClientes_bkps";
import { toast } from "sonner";

const formSchema = z.object({
  
    idCliente: z.any(),
  
    apelido: z.any(),
  
    idGrupo: z.any(),
  
    idSituacao: z.any(),
  
    nome: z.any(),
  
    email: z.any(),
  
    telCelular: z.any(),
  
    telCelularObs: z.any(),
  
    telFixo: z.any(),
  
    telFixoObs: z.any(),
  
    dataNascimento: z.any(),
  
    cpf: z.any(),
  
    rg: z.any(),
  
    ctps: z.any(),
  
    endereco: z.any(),
  
    numero: z.any(),
  
    complemento: z.any(),
  
    bairro: z.any(),
  
    cep: z.any(),
  
    estado: z.any(),
  
    cidade: z.any(),
  
    dataIngresso: z.any(),
  
    observacoes: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    naturalEstado: z.any(),
  
    naturalCidade: z.any(),
  
    nomeDaMae: z.any(),
  
    dib: z.any(),
  
    dibData: z.any(),
  
    dibIdTipoBeneficio: z.any(),
  
    idCargo: z.any(),
  
    telCelular2: z.any(),
  
    telCelular2Obs: z.any(),
  
    telFixo2: z.any(),
  
    telFixo2Obs: z.any(),
  
    cnpj: z.any(),
  
    ie: z.any(),
  
    idFornecedor: z.any(),
  
    incluidoPor: z.any(),
  
    inssAgendado: z.any(),
  
    inssData: z.any(),
  
    inssIdTipoBeneficio: z.any(),
  
    inssIdPosto: z.any(),
  
    inssResultado: z.any(),
  
    prospect: z.any(),
  
    idLocalAtendido: z.any(),
  
    whatsapp: z.any(),
  
    pastaFTP: z.any(),
  
    inssResponsavel: z.any(),
  
    responsavelPendencia: z.any(),
  
    comoChegou: z.any(),
  
    inssProtocolo: z.any(),
  
    inssTsInclusao: z.any(),
  
    inssIdUsuarioInclusao: z.any(),
  
    foto: z.any(),
  
    followBloqueadoAte: z.any(),
  
    falecido: z.any(),
  
    senhaINSSDigital: z.any(),
  
    idPrioridade: z.any(),
  
    instagram: z.any(),
  
    rgOrgaoExp: z.any(),
  
    nacionalidade: z.any(),
  
    estadocivil: z.any(),
  
    dcb: z.any(),
  
    dcbData: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvClientes_bkpFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvClientes_bkpForm({
  isOpen,
  onClose,
  initialValues,
}: AdvClientes_bkpFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateAdvClientes_bkp();
const updateMutation = useUpdateAdvClientes_bkp();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advClientes_bkp updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advClientes_bkp created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advclientes_bkp:", error);
    toast.error(error.message || "Failed to save advclientes_bkp");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advClientes_bkp": "Create advClientes_bkp" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advclientes_bkp." : "Fill in the details to create a new advclientes_bkp." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente</Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="apelido" > apelido</Label>

<Input id="apelido" {...register("apelido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idGrupo" > idGrupo</Label>

<Input id="idGrupo" type = "number" step = "any" {...register("idGrupo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idSituacao" > idSituacao</Label>

<Input id="idSituacao" type = "number" step = "any" {...register("idSituacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nome" > nome</Label>

<Input id="nome" {...register("nome") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="email" > email</Label>

<Input id="email" {...register("email") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telCelular" > telCelular</Label>

<Input id="telCelular" {...register("telCelular") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telCelularObs" > telCelularObs</Label>

<Input id="telCelularObs" {...register("telCelularObs") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telFixo" > telFixo</Label>

<Input id="telFixo" {...register("telFixo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telFixoObs" > telFixoObs</Label>

<Input id="telFixoObs" {...register("telFixoObs") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataNascimento" > dataNascimento</Label>

<Input id="dataNascimento" {...register("dataNascimento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cpf" > cpf</Label>

<Input id="cpf" {...register("cpf") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="rg" > rg</Label>

<Input id="rg" {...register("rg") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ctps" > ctps</Label>

<Input id="ctps" {...register("ctps") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="endereco" > endereco</Label>

<Input id="endereco" {...register("endereco") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="numero" > numero</Label>

<Input id="numero" {...register("numero") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="complemento" > complemento</Label>

<Input id="complemento" {...register("complemento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bairro" > bairro</Label>

<Input id="bairro" {...register("bairro") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cep" > cep</Label>

<Input id="cep" {...register("cep") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="estado" > estado</Label>

<Input id="estado" {...register("estado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cidade" > cidade</Label>

<Input id="cidade" {...register("cidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataIngresso" > dataIngresso</Label>

<Input id="dataIngresso" {...register("dataIngresso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="observacoes" > observacoes</Label>

<Input id="observacoes" {...register("observacoes") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ativo" > ativo * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="ativo"
checked = { watch("ativo") }
onCheckedChange = {(checked) => setValue("ativo", !!checked)}
              />
  < label htmlFor = "ativo" className = "text-sm font-normal" >
    { watch("ativo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao * </Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="naturalEstado" > naturalEstado</Label>

<Input id="naturalEstado" {...register("naturalEstado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="naturalCidade" > naturalCidade</Label>

<Input id="naturalCidade" {...register("naturalCidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nomeDaMae" > nomeDaMae</Label>

<Input id="nomeDaMae" {...register("nomeDaMae") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dib" > dib * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="dib"
checked = { watch("dib") }
onCheckedChange = {(checked) => setValue("dib", !!checked)}
              />
  < label htmlFor = "dib" className = "text-sm font-normal" >
    { watch("dib") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="dibData" > dibData</Label>

<Input id="dibData" {...register("dibData") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dibIdTipoBeneficio" > dibIdTipoBeneficio</Label>

<Input id="dibIdTipoBeneficio" type = "number" step = "any" {...register("dibIdTipoBeneficio") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCargo" > idCargo</Label>

<Input id="idCargo" type = "number" step = "any" {...register("idCargo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telCelular2" > telCelular2</Label>

<Input id="telCelular2" {...register("telCelular2") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telCelular2Obs" > telCelular2Obs</Label>

<Input id="telCelular2Obs" {...register("telCelular2Obs") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telFixo2" > telFixo2</Label>

<Input id="telFixo2" {...register("telFixo2") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telFixo2Obs" > telFixo2Obs</Label>

<Input id="telFixo2Obs" {...register("telFixo2Obs") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cnpj" > cnpj</Label>

<Input id="cnpj" {...register("cnpj") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ie" > ie</Label>

<Input id="ie" {...register("ie") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idFornecedor" > idFornecedor</Label>

<Input id="idFornecedor" type = "number" step = "any" {...register("idFornecedor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="incluidoPor" > incluidoPor</Label>

<Input id="incluidoPor" {...register("incluidoPor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssAgendado" > inssAgendado * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="inssAgendado"
checked = { watch("inssAgendado") }
onCheckedChange = {(checked) => setValue("inssAgendado", !!checked)}
              />
  < label htmlFor = "inssAgendado" className = "text-sm font-normal" >
    { watch("inssAgendado") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="inssData" > inssData</Label>

<Input id="inssData" {...register("inssData") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssIdTipoBeneficio" > inssIdTipoBeneficio</Label>

<Input id="inssIdTipoBeneficio" type = "number" step = "any" {...register("inssIdTipoBeneficio") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssIdPosto" > inssIdPosto</Label>

<Input id="inssIdPosto" type = "number" step = "any" {...register("inssIdPosto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssResultado" > inssResultado</Label>

<Input id="inssResultado" {...register("inssResultado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="prospect" > prospect * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="prospect"
checked = { watch("prospect") }
onCheckedChange = {(checked) => setValue("prospect", !!checked)}
              />
  < label htmlFor = "prospect" className = "text-sm font-normal" >
    { watch("prospect") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="idLocalAtendido" > idLocalAtendido</Label>

<Input id="idLocalAtendido" type = "number" step = "any" {...register("idLocalAtendido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="whatsapp" > whatsapp</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="whatsapp"
checked = { watch("whatsapp") }
onCheckedChange = {(checked) => setValue("whatsapp", !!checked)}
              />
  < label htmlFor = "whatsapp" className = "text-sm font-normal" >
    { watch("whatsapp") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="pastaFTP" > pastaFTP</Label>

<Input id="pastaFTP" {...register("pastaFTP") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssResponsavel" > inssResponsavel</Label>

<Input id="inssResponsavel" type = "number" step = "any" {...register("inssResponsavel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="responsavelPendencia" > responsavelPendencia</Label>

<Input id="responsavelPendencia" type = "number" step = "any" {...register("responsavelPendencia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="comoChegou" > comoChegou</Label>

<Input id="comoChegou" type = "number" step = "any" {...register("comoChegou") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssProtocolo" > inssProtocolo</Label>

<Input id="inssProtocolo" {...register("inssProtocolo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssTsInclusao" > inssTsInclusao</Label>

<Input id="inssTsInclusao" type = "date" {...register("inssTsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssIdUsuarioInclusao" > inssIdUsuarioInclusao</Label>

<Input id="inssIdUsuarioInclusao" type = "number" step = "any" {...register("inssIdUsuarioInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="foto" > foto</Label>

<Input id="foto" {...register("foto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="followBloqueadoAte" > followBloqueadoAte</Label>

<Input id="followBloqueadoAte" {...register("followBloqueadoAte") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="falecido" > falecido * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="falecido"
checked = { watch("falecido") }
onCheckedChange = {(checked) => setValue("falecido", !!checked)}
              />
  < label htmlFor = "falecido" className = "text-sm font-normal" >
    { watch("falecido") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="senhaINSSDigital" > senhaINSSDigital</Label>

<Input id="senhaINSSDigital" {...register("senhaINSSDigital") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idPrioridade" > idPrioridade</Label>

<Input id="idPrioridade" type = "number" step = "any" {...register("idPrioridade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="instagram" > instagram</Label>

<Input id="instagram" {...register("instagram") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="rgOrgaoExp" > rgOrgaoExp</Label>

<Input id="rgOrgaoExp" {...register("rgOrgaoExp") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nacionalidade" > nacionalidade</Label>

<Input id="nacionalidade" {...register("nacionalidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="estadocivil" > estadocivil</Label>

<Input id="estadocivil" {...register("estadocivil") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dcb" > dcb</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="dcb"
checked = { watch("dcb") }
onCheckedChange = {(checked) => setValue("dcb", !!checked)}
              />
  < label htmlFor = "dcb" className = "text-sm font-normal" >
    { watch("dcb") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="dcbData" > dcbData</Label>

<Input id="dcbData" {...register("dcbData") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
