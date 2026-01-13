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
  import { useCreateAdvCompromissos, useUpdateAdvCompromissos } from "@/lib/abp/hooks/useAdvCompromissoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idCompromisso: z.any(),
  
    idTipoCompromisso: z.any(),
  
    idProcesso: z.any(),
  
    dataPublicacao: z.any(),
  
    dataPrazoInterno: z.any(),
  
    dataPrazoFatal: z.any(),
  
    descricao: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    incluidoPor: z.any(),
  
    alteradoPor: z.any(),
  
    idAgendamentoINSS: z.any(),
  
    pauta: z.any(),
  
    pautaIdUsuarioResp: z.any(),
  
    pautaRespAceite: z.any(),
  
    horarioInicial: z.any(),
  
    horarioFinal: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvCompromissosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvCompromissosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvCompromissosFormProps) {
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

  const createMutation = useCreateAdvCompromissos();
const updateMutation = useUpdateAdvCompromissos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advCompromissos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advCompromissos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advcompromissos:", error);
    toast.error(error.message || "Failed to save advcompromissos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advCompromissos": "Create advCompromissos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advcompromissos." : "Fill in the details to create a new advcompromissos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idCompromisso" > idCompromisso</Label>

<Input id="idCompromisso" type = "number" step = "any" {...register("idCompromisso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipoCompromisso" > idTipoCompromisso * </Label>

<Input id="idTipoCompromisso" type = "number" step = "any" {...register("idTipoCompromisso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso</Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataPublicacao" > dataPublicacao</Label>

<Input id="dataPublicacao" {...register("dataPublicacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataPrazoInterno" > dataPrazoInterno</Label>

<Input id="dataPrazoInterno" {...register("dataPrazoInterno") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataPrazoFatal" > dataPrazoFatal</Label>

<Input id="dataPrazoFatal" {...register("dataPrazoFatal") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descricao" > descricao</Label>

<Input id="descricao" {...register("descricao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ativo" > ativo</Label>

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
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="incluidoPor" > incluidoPor</Label>

<Input id="incluidoPor" {...register("incluidoPor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="alteradoPor" > alteradoPor</Label>

<Input id="alteradoPor" {...register("alteradoPor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idAgendamentoINSS" > idAgendamentoINSS</Label>

<Input id="idAgendamentoINSS" type = "number" step = "any" {...register("idAgendamentoINSS") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="pauta" > pauta</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="pauta"
checked = { watch("pauta") }
onCheckedChange = {(checked) => setValue("pauta", !!checked)}
              />
  < label htmlFor = "pauta" className = "text-sm font-normal" >
    { watch("pauta") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="pautaIdUsuarioResp" > pautaIdUsuarioResp</Label>

<Input id="pautaIdUsuarioResp" type = "number" step = "any" {...register("pautaIdUsuarioResp") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="pautaRespAceite" > pautaRespAceite</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="pautaRespAceite"
checked = { watch("pautaRespAceite") }
onCheckedChange = {(checked) => setValue("pautaRespAceite", !!checked)}
              />
  < label htmlFor = "pautaRespAceite" className = "text-sm font-normal" >
    { watch("pautaRespAceite") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="horarioInicial" > horarioInicial</Label>

<Input id="horarioInicial" type = "number" step = "any" {...register("horarioInicial") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="horarioFinal" > horarioFinal</Label>

<Input id="horarioFinal" type = "number" step = "any" {...register("horarioFinal") } />

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
