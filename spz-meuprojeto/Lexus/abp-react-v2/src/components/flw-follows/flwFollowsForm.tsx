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
  import { useCreateFlwFollows, useUpdateFlwFollows } from "@/lib/abp/hooks/useFlwFollowses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idFollow: z.any(),
  
    idCliente: z.any(),
  
    idAcao: z.any(),
  
    idTipo: z.any(),
  
    idUsuario: z.any(),
  
    data: z.any(),
  
    horario: z.any(),
  
    comentario: z.any(),
  
    finalizado: z.any(),
  
    dataFinalizacao: z.any(),
  
    horarioFinalizacao: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    idOportunidade: z.any(),
  
    chegou: z.any(),
  
    tsChegou: z.any(),
  
    naoComparecimento: z.any(),
  
    prioridade: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FlwFollowsFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FlwFollowsForm({
  isOpen,
  onClose,
  initialValues,
}: FlwFollowsFormProps) {
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

  const createMutation = useCreateFlwFollows();
const updateMutation = useUpdateFlwFollows();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("flwFollows updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("flwFollows created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save flwfollows:", error);
    toast.error(error.message || "Failed to save flwfollows");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit flwFollows": "Create flwFollows" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the flwfollows." : "Fill in the details to create a new flwfollows." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idFollow" > idFollow</Label>

<Input id="idFollow" type = "number" step = "any" {...register("idFollow") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente * </Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idAcao" > idAcao * </Label>

<Input id="idAcao" type = "number" step = "any" {...register("idAcao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipo" > idTipo * </Label>

<Input id="idTipo" type = "number" step = "any" {...register("idTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario * </Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="data" > data * </Label>

<Input id="data" {...register("data") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="horario" > horario</Label>

<Input id="horario" type = "number" step = "any" {...register("horario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="comentario" > comentario</Label>

<Input id="comentario" {...register("comentario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="finalizado" > finalizado</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="finalizado"
checked = { watch("finalizado") }
onCheckedChange = {(checked) => setValue("finalizado", !!checked)}
              />
  < label htmlFor = "finalizado" className = "text-sm font-normal" >
    { watch("finalizado") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="dataFinalizacao" > dataFinalizacao</Label>

<Input id="dataFinalizacao" {...register("dataFinalizacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="horarioFinalizacao" > horarioFinalizacao</Label>

<Input id="horarioFinalizacao" type = "number" step = "any" {...register("horarioFinalizacao") } />

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
  <Label htmlFor="idOportunidade" > idOportunidade</Label>

<Input id="idOportunidade" type = "number" step = "any" {...register("idOportunidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="chegou" > chegou</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="chegou"
checked = { watch("chegou") }
onCheckedChange = {(checked) => setValue("chegou", !!checked)}
              />
  < label htmlFor = "chegou" className = "text-sm font-normal" >
    { watch("chegou") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsChegou" > tsChegou</Label>

<Input id="tsChegou" type = "date" {...register("tsChegou") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="naoComparecimento" > naoComparecimento</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="naoComparecimento"
checked = { watch("naoComparecimento") }
onCheckedChange = {(checked) => setValue("naoComparecimento", !!checked)}
              />
  < label htmlFor = "naoComparecimento" className = "text-sm font-normal" >
    { watch("naoComparecimento") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="prioridade" > prioridade</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="prioridade"
checked = { watch("prioridade") }
onCheckedChange = {(checked) => setValue("prioridade", !!checked)}
              />
  < label htmlFor = "prioridade" className = "text-sm font-normal" >
    { watch("prioridade") ?"Enabled": "Disabled" }
    </label>
    </div>

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
