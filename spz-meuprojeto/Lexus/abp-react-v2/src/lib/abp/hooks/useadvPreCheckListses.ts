import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPreCheckListsesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPreCheckListses(input: GetAdvPreCheckListsesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPreCheckListses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-check-lists", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllAdvPreCheckListses() {
  return useQuery({
    queryKey: ["advPreCheckListses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-check-lists", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPreCheckLists(id: string) {
  return useQuery({
    queryKey: ["advPreCheckLists", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pre-check-lists/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPreCheckLists() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pre-check-lists", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreCheckListses"] });
    },
  });
}

export function useUpdateAdvPreCheckLists() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pre-check-lists/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPreCheckListses"] });
      queryClient.invalidateQueries({ queryKey: ["advPreCheckLists", data.id] });
    },
  });
}

export function useDeleteAdvPreCheckLists() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pre-check-lists/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreCheckListses"] });
    },
  });
}


