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
  import { useCreateAdvClientesINSS, useUpdateAdvClientesINSS } from "@/lib/abp/hooks/useAdvClientesINSSes";
import { toast } from "sonner";

const formSchema = z.object({
  
    idInssAgendado: z.any(),
  
    idCliente: z.any(),
  
    inssAgendado: z.any(),
  
    inssData: z.any(),
  
    inssIdTipoBeneficio: z.any(),
  
    inssIdPosto: z.any(),
  
    inssResultado: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    inssResultadoIndicadorOculto: z.any(),
  
    inssResponsavel: z.any(),
  
    inssProtocolo: z.any(),
  
    inssIdUsuarioInclusao: z.any(),
  
    idStatus: z.any(),
  
    dataFinalizacao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvClientesINSSFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvClientesINSSForm({
  isOpen,
  onClose,
  initialValues,
}: AdvClientesINSSFormProps) {
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

  const createMutation = useCreateAdvClientesINSS();
const updateMutation = useUpdateAdvClientesINSS();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advClientesINSS updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advClientesINSS created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advclientesinss:", error);
    toast.error(error.message || "Failed to save advclientesinss");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advClientesINSS": "Create advClientesINSS" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advclientesinss." : "Fill in the details to create a new advclientesinss." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idInssAgendado" > idInssAgendado</Label>

<Input id="idInssAgendado" type = "number" step = "any" {...register("idInssAgendado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente * </Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssAgendado" > inssAgendado</Label>

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
  <Label htmlFor="tsInclusao" > tsInclusao * </Label>

<Input id="tsInclusao" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssResultadoIndicadorOculto" > inssResultadoIndicadorOculto</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="inssResultadoIndicadorOculto"
checked = { watch("inssResultadoIndicadorOculto") }
onCheckedChange = {(checked) => setValue("inssResultadoIndicadorOculto", !!checked)}
              />
  < label htmlFor = "inssResultadoIndicadorOculto" className = "text-sm font-normal" >
    { watch("inssResultadoIndicadorOculto") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="inssResponsavel" > inssResponsavel</Label>

<Input id="inssResponsavel" type = "number" step = "any" {...register("inssResponsavel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssProtocolo" > inssProtocolo</Label>

<Input id="inssProtocolo" {...register("inssProtocolo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="inssIdUsuarioInclusao" > inssIdUsuarioInclusao</Label>

<Input id="inssIdUsuarioInclusao" type = "number" step = "any" {...register("inssIdUsuarioInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idStatus" > idStatus</Label>

<Input id="idStatus" type = "number" step = "any" {...register("idStatus") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataFinalizacao" > dataFinalizacao</Label>

<Input id="dataFinalizacao" {...register("dataFinalizacao") } />

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
