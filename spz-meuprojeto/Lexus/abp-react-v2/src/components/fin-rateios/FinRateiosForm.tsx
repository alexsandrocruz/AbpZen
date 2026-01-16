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
  import { useCreateFinRateios, useUpdateFinRateios } from "@/lib/abp/hooks/useFinRateioses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idRateio: z.any(),
  
    idLancamento: z.any(),
  
    idCentroCusto: z.any(),
  
    idCentroResultado: z.any(),
  
    percentualCC: z.any(),
  
    percentualCR: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    idUnidade: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FinRateiosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FinRateiosForm({
  isOpen,
  onClose,
  initialValues,
}: FinRateiosFormProps) {
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

  const createMutation = useCreateFinRateios();
const updateMutation = useUpdateFinRateios();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("finRateios updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("finRateios created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save finrateios:", error);
    toast.error(error.message || "Failed to save finrateios");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit finRateios": "Create finRateios" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the finrateios." : "Fill in the details to create a new finrateios." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idRateio" > idRateio</Label>

<Input id="idRateio" type = "number" step = "any" {...register("idRateio") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idLancamento" > idLancamento * </Label>

<Input id="idLancamento" type = "number" step = "any" {...register("idLancamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCentroCusto" > idCentroCusto * </Label>

<Input id="idCentroCusto" type = "number" step = "any" {...register("idCentroCusto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCentroResultado" > idCentroResultado</Label>

<Input id="idCentroResultado" type = "number" step = "any" {...register("idCentroResultado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="percentualCC" > percentualCC</Label>

<Input id="percentualCC" type = "number" step = "any" {...register("percentualCC") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="percentualCR" > percentualCR</Label>

<Input id="percentualCR" type = "number" step = "any" {...register("percentualCR") } />

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
  <Label htmlFor="idUnidade" > idUnidade</Label>

<Input id="idUnidade" type = "number" step = "any" {...register("idUnidade") } />

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
