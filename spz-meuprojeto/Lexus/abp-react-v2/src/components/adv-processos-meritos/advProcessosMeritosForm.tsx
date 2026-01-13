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
  import { useCreateAdvProcessosMeritos, useUpdateAdvProcessosMeritos } from "@/lib/abp/hooks/useAdvProcessosMeritoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idProcessoMerito: z.any(),
  
    idProcesso: z.any(),
  
    idMerito: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvProcessosMeritosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvProcessosMeritosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvProcessosMeritosFormProps) {
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

  const createMutation = useCreateAdvProcessosMeritos();
const updateMutation = useUpdateAdvProcessosMeritos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advProcessosMeritos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advProcessosMeritos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advprocessosmeritos:", error);
    toast.error(error.message || "Failed to save advprocessosmeritos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advProcessosMeritos": "Create advProcessosMeritos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advprocessosmeritos." : "Fill in the details to create a new advprocessosmeritos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idProcessoMerito" > idProcessoMerito</Label>

<Input id="idProcessoMerito" type = "number" step = "any" {...register("idProcessoMerito") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso * </Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idMerito" > idMerito * </Label>

<Input id="idMerito" type = "number" step = "any" {...register("idMerito") } />

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
